import React from 'react';
import classes from './Input.module.css'

const Input = ({inputRef, ...props}) => {
    return (
        <input className={classes.Input} ref={inputRef} {...props}/>
    );
};

export default Input;