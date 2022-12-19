import './App.css';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import BookList from "./components/BookList/BookList";
import BookEditList from "./components/BookEditList/BookEditList";
import NavBar from "./components/NavBar/NavBar";


function App() {
  return (
    <div className="App">
        <BrowserRouter>
            <NavBar/>
            <Routes>
                <Route path='/' element={<BookList/>}/>
                <Route path='/management/books' element={<BookEditList/>}/>
            </Routes>
        </BrowserRouter>
    </div>
  );
}

export default App;
