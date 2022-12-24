import React, {Fragment} from 'react';
import classes from './NavBar.module.css'
import {Link, useNavigate} from "react-router-dom";
import {AuthContext} from "../../utils/AuthContext";
import UserService from "../../services/UserService";

const NavBar = () => {
    const navigate = useNavigate();
    const {token, removeToken, authUserId} = React.useContext(AuthContext);
    const [authUser, setAuthUser] = React.useState({});

    React.useEffect(() => {
        if (authUserId) {
            fetchUserInfo(authUserId);
        }
    }, [authUserId])

    async function fetchUserInfo(id) {
        setAuthUser(await UserService.getById(id));
    }

    async function logout() {
        removeToken();
        await UserService.logout();
        navigate('/');
    }

    return (
        <div className={classes.NavBar}>
            <h1 className={classes.Header}>Books Store</h1>
            <div className={classes.Links}>
                <Link to='/'>Books</Link>
                <Link to='/management/books'>Management</Link>
            </div>
            <div className={classes.LoginContr}>
                {token === null ?
                    <Fragment>
                        <button onClick={() => navigate('/login')}>Log in</button>
                        <button onClick={() => navigate('/register')}>Register</button>
                    </Fragment>
                    :
                    <Fragment>
                        <h4>Welcome, {authUser.username}</h4>
                        <button onClick={logout}>Log out</button>
                    </Fragment>
                }

            </div>
        </div>
    );
};

export default NavBar;