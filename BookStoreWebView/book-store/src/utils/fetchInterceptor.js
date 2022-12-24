const {fetch: originalFetch} = window;
window.originalFetch = originalFetch;

export default function fetchInterceptor (token, saveToken, removeToken) {
    const refreshUrl = process.env.REACT_APP_SERVER_API_URL + 'auth/refresh-token';

    window.fetch = async (...args) => {
        const [resource, config] = args;

        const response = await originalFetch(resource, config);

        if (response.status === 401) {
            const res = await originalFetch(refreshUrl, {
                credentials: 'include',
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({refreshToken: token})
            });

            if (res.status === 200) {
                const resToken = await res.json();
                saveToken(resToken);

                return await  originalFetch(resource, config);
            } else if (res.status === 401) {
                removeToken();
            }
        }

        return response;
    }
}