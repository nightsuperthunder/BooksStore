export default class BookService {
    static async getAll() {
        const apiUrl = process.env.REACT_APP_SERVER_API_URL;

        return await fetch(apiUrl + 'books');
    }

    static async create(book) {
        const apiUrl = process.env.REACT_APP_SERVER_API_URL;

        const response = await fetch(apiUrl + 'books', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(book)
        });

        if (response.status === 200){
            return await response.json()
        }

        throw new Error();
    }

    static async delete(id) {
        const apiUrl = process.env.REACT_APP_SERVER_API_URL;

        await fetch(apiUrl + 'books/' + id, {
            method: 'DELETE'
        });
    }
}