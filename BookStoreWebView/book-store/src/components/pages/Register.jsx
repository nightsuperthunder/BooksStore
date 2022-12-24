import React from 'react';
import Input from "../Input/Input";
import UserService from "../../services/UserService";
import {AuthContext} from "../../utils/AuthContext";
import {useNavigate} from "react-router-dom";

const Register = () => {
    const [user, setUser] = React.useState({username: '', email: '', password: ''});
    const {saveToken} = React.useContext(AuthContext);
    const navigate = useNavigate();


    async function register(e){
        e.preventDefault();
        const response = await UserService.register(user);
        console.log(response)
        saveToken(response);
        navigate('/');
    }

    return (
        <div>
            <h1>Register page</h1>
            <form onSubmit={register}>
                <h4>Username</h4>
                <Input value={user.username} onChange={e => setUser({...user, username: e.target.value})} type='text'/>
                <h4>E-mail</h4>
                <Input value={user.email} onChange={e => setUser({...user, email: e.target.value})} type='email'/>
                <h4>Password</h4>
                <Input value={user.password} onChange={e => setUser({...user, password: e.target.value})} type='password'/>
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default Register;