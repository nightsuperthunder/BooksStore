import React from 'react';
import classes from './NavBar.module.css'
import {Link} from "react-router-dom";

const NavBar = () => {
    return (
        <div className={classes.NavBar}>
            <h1 className={classes.Header}>Books Store</h1>
            <div className={classes.Links}>
                <Link to='/'>Books</Link>
                <Link to='/management/books'>Management</Link>
            </div>
            <div className={classes.LoginContr}>
                <h4>Welcome, user</h4>
                <button>Login</button>
            </div>
        </div>
    );
};

export default NavBar;