import './App.scss';
import fbConnection from "../helpers/data/connection";
import Customers from '../components/pages/Customers/Customers';
import Login from '../components/pages/Login/Login';
import {BrowserRouter, Route, Redirect, Switch} from 'react-router-dom';

fbConnection();

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Switch>
          <Route path="/Login" component={Login}></Route>
          <Route path="/Customers" component={Customers}></Route>
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
