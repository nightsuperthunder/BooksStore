export default class UserService {
    static async login(email, password) {
        const apiUrl = process.env.REACT_APP_SERVER_API_URL;

        const response = await fetch(apiUrl + 'auth/login', {
            credentials: 'include',
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({email: email, password: password})
        });

        return {response: response, json: await response.json()};
    }

    static async register(user) {
        const apiUrl = process.env.REACT_APP_SERVER_API_URL;

        const response = await fetch(apiUrl + 'auth/register', {
            credentials: 'include',
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        });

        return await response.json();
    }

    static async logout() {
        const apiUrl = process.env.REACT_APP_SERVER_API_URL;

        return await fetch(apiUrl + 'auth/logout', {
            credentials: 'include',
            method: 'POST'
        });
    }

    static async getById(id) {
        const apiUrl = process.env.REACT_APP_SERVER_API_URL;

        const response = await fetch(apiUrl + 'users/' + id, {
            credentials: 'include',
            method: 'GET'
        })

        return await response.json();
    }
}