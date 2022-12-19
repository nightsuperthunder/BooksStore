import React from 'react';
import BookService from "../../services/BookService";
import BookItem from "../BookItem/BookItem";


const BookList = () => {
    const [books, setBooks] = React.useState([]);

    React.useEffect(() => {
       fetchBooks()
    }, []);

    async function fetchBooks() {
        const books = await (await BookService.getAll()).json();
        setBooks(books);
    }

    return (
        <div>
            {books.map((b) =>
                <BookItem key={b.id} book={b}/>
            )}
        </div>
    );
};

export default BookList;