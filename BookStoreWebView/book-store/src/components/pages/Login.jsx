import React from 'react';
import Input from "../Input/Input";
import {useNavigate} from "react-router-dom";
import UserService from "../../services/UserService";
import {AuthContext} from "../../utils/AuthContext";

const Login = () => {
    const [email, setEmail] = React.useState('');
    const [password, setPassword] = React.useState('');
    const navigate = useNavigate();
    const {saveToken} = React.useContext(AuthContext);

    async function login(e) {
        e.preventDefault();
        const {response, json} = await UserService.login(email, password);
        if (response.status !== 200){
            alert("Incorrect credentials");
            return;
        }

        saveToken(json)

        navigate('/');
    }

    return (
        <div>
            <h1>Login page</h1>
            <form onSubmit={login}>
                <h4>E-mail</h4>
                <Input value={email} onChange={e => setEmail(e.target.value)} type='email'/>
                <h4>Password</h4>
                <Input value={password} onChange={e => setPassword(e.target.value)} type='password'/>
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default Login;