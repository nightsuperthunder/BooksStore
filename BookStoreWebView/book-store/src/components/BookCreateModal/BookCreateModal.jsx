import React from 'react';
import classes from './BookCreateModal.module.css'
import Input from "../Input/Input";
import BookService from "../../services/BookService";

const BookCreateModal = ({visible, setVisible, books, setBooks}) => {
    const [book, setBook] = React.useState({name: '', description: '', isbn: '', author: '', year: '', price: '', previewImg: ''});
    const imgInputRef = React.useRef(null)

    const modalClasses = [classes.Modal];
    if (visible) {
        modalClasses.push(classes.Active);
    }

    async function addBook(e) {
        e.preventDefault();
        const createdBook = await BookService.create(book);
        console.log(createdBook)
        setBooks([...books, createdBook])
        hideAndClear();
    }

    function hideAndClear() {
        setVisible(false);
        setBook({name: '', description: '', isbn: '', author: '', year: '', price: '', previewImg: ''});
        imgInputRef.current.value = null;
    }

    return (
        <div className={modalClasses.join(' ')} onClick={hideAndClear}>
            <div className={classes.ModalContent} onClick={(e) => e.stopPropagation()}>
                <form onSubmit={addBook}>
                    <Input value={book.name}
                    onChange={e => setBook({...book, name: e.target.value})}
                    type='text' placeholder='Book name' required/>
                    <Input value={book.description}
                           onChange={e => setBook({...book, description: e.target.value})}
                           type='text' placeholder='Description'/>
                    <Input value={book.isbn}
                           onChange={e => setBook({...book, isbn: e.target.value})}
                           type='text' placeholder='ISBN' required/>
                    <Input value={book.author}
                           onChange={e => setBook({...book, author: e.target.value})}
                           type='text' placeholder='Author' required/>
                    <Input value={book.year}
                           onChange={e => setBook({...book, year: e.target.value})}
                           type='text' placeholder='Year' required/>
                    <Input value={book.price}
                           onChange={e => setBook({...book, price: e.target.value})}
                           type='text' placeholder='Price' required/>
                    <Input type='file' inputRef={imgInputRef} accept='image/*'
                           onChange={(e) => {
                               const reader = new FileReader();
                               reader.onload = () => setBook({...book, previewImg: reader.result});
                               if (e.target.files[0]) {
                                   reader.readAsDataURL(e.target.files[0])
                               }
                           }}
                    />
                    <button type='submit'>Submit</button>
                </form>
            </div>
        </div>
    );
};

export default BookCreateModal;