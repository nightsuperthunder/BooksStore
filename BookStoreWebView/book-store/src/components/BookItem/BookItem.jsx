import React from 'react';
import classes from "./BookItem.module.css";

const BookItem = ({book}) => {
    return (
        <div className={classes.BookItm}>
            <div className={classes.BookName}><h2>{book.name}</h2></div>
            <div className={classes.BookImg}><img src={book.previewImg} alt={book.name}/></div>
            <div className={classes.BookDesc}>{book.description}</div>
            <div className={classes.BookYear}>Year: {book.year}</div>
            <div className={classes.BookIsbn}>ISBN: {book.isbn}</div>
            <div className={classes.BookBuy}>
                <button>Buy</button>
            </div>
        </div>
    );
};

export default BookItem;