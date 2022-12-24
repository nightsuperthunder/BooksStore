import './App.css';
import React from "react";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import BookList from "./components/BookList/BookList";
import BookEditList from "./components/BookEditList/BookEditList";
import NavBar from "./components/NavBar/NavBar";
import Login from "./components/pages/Login";
import {AuthContext} from "./utils/AuthContext";
import useToken from "./utils/useToken";
import fetchInterceptor from "./utils/fetchInterceptor";
import Register from "./components/pages/Register";

function App() {
    const [token, saveToken, removeToken, authUserId] = useToken();

    fetchInterceptor(token, saveToken, removeToken);

    return (
        <div className="App">
            <AuthContext.Provider value={{
                token,
                saveToken,
                removeToken,
                authUserId
            }}>
                <BrowserRouter>
                    <NavBar/>
                    <Routes>
                        <Route path='/' element={<BookList/>}/>
                        <Route path='/management/books' element={<BookEditList/>}/>
                        <Route path='/login' element={<Login/>}/>
                        <Route path='/register' element={<Register/>}/>
                    </Routes>
                </BrowserRouter>
            </AuthContext.Provider>
        </div>
    );
}

export default App;
