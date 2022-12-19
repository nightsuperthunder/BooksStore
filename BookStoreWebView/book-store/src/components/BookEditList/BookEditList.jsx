import React from 'react';
import BookService from "../../services/BookService";
import classes from "./BookEditList.module.css";
import BookCreateModal from "../BookCreateModal/BookCreateModal";

const BookEditList = () => {
    const [books, setBooks] = React.useState([]);
    const [modal, setModal] = React.useState(false);

    React.useEffect(() => {
        fetchBooks()

    }, []);

    async function fetchBooks() {
        const books = await (await BookService.getAll()).json();
        setBooks(books);
    }

    async function deleteBook(id) {
        await BookService.delete(id);
        setBooks(books.filter(b => b.id !== id))
    }

    return (
        <div>
            <button onClick={() => setModal(true)}>Add Book</button>
            <BookCreateModal visible={modal} setVisible={setModal} books={books} setBooks={setBooks}/>
            {books.map((b) =>
                <div key={b.id} className={classes.BookItm}>
                    <div style={{gridArea: 1 / 2 / 2}}>{b.name}</div>
                    <div style={{gridArea: 1 / 2 / 2 / 3}}>{b.isbn}</div>
                    <div style={{gridArea: 1 / 3 / 2 / 4}}>
                        <button onClick={() => deleteBook(b.id)}>Delete</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default BookEditList;