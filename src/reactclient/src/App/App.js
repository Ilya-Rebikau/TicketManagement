import {Events} from "../components/Events/Events";
import {EventAreas} from '../components/EventAreas/EventAreas';
import {EventSeats} from '../components/EventSeats/EventSeats';
import {Tickets} from '../components/Tickets/Tickets';
import {Account} from '../components/Account/Account';
import { appsettings } from "./appsettings";
import {BrowserRouter, Route, Routes, NavLink} from 'react-router-dom';
import jwt from 'jwt-decode';
import Cookies from 'js-cookie'
import React  from 'react';

function getRoles(){
  const roles = [];
  if (Cookies.get('JwtTokenCookie') !== undefined){
    const jwtToken = Cookies.get('JwtTokenCookie');
    const user = jwt(jwtToken);
    const userData = [];
    Object.keys(user).forEach(key => userData.push({name: key, value: user[key]}));
    if(Array.isArray(userData[1].value)){
      Object.keys(userData[1].value).forEach(key => roles.push(userData[1].value[key]));
    }
    else{
      roles.push(userData[1].value);
    }
  }
  return roles;
}

function App() {
  const roles = getRoles();
  return (
    <BrowserRouter>
      <div className="App">
        <header>
          <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-success border-bottom box-shadow mb-3">
            <div className="container">
              <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                              aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
              </button>
              <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                <ul className="navbar-nav flex-grow-1">
                    <li className="nav-item">
                      <NavLink className="nav-link text-white" to="/">Events</NavLink>
                    </li>
                    {roles.includes('admin') || roles.includes('venue manager')?
                      <li className="nav-item">
                        <a className="nav-link text-white" href={appsettings.ApsWebApp + 'venues'}>Venues</a>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('venue manager')?
                      <li className="nav-item">
                        <a className="nav-link text-white" href={appsettings.ApsWebApp + 'layouts'}>Layouts</a>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('venue manager')?
                      <li className="nav-item">
                        <a className="nav-link text-white" href={appsettings.ApsWebApp + 'areas'}>Areas</a>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('venue manager')?
                      <li className="nav-item">
                        <a className="nav-link text-white" href={appsettings.ApsWebApp + 'seats'}>Seats</a>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('event manager')?
                      <li className="nav-item">
                        <NavLink className="nav-link text-white" to="/eventseats">EventSeats</NavLink>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('event manager')?
                      <li className="nav-item">
                        <NavLink className="nav-link text-white" to="/eventareas">EventAreas</NavLink>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('event manager')?
                      <li className="nav-item">
                        <NavLink className="nav-link text-white" to="/tickets">Tickets</NavLink>
                      </li>
                     :null}
                    {roles.includes('admin')?
                      <li className="nav-item">
                        <a className="nav-link text-white" href={appsettings.ApsWebApp + 'users'}>Users</a>
                      </li>
                     :null}
                  </ul>
                  {roles.includes("admin") || roles.includes("event manager") || roles.includes("user") || roles.includes("venue manager")?
                    <ul className="navbar-nav flex-grow-1 justify-content-end">
                      <li className="nav-item justify-content-end">
                        <NavLink className="nav-link text-white" to="/account/personalaccount">Personal account</NavLink>
                      </li>
                    </ul>:null}
              </div>
            </div>
          </nav>
        </header>
        <div className="container">
          <main role="main" className="pb-3">
            <Routes>
              <Route path='/' element={<Events roles={roles}/>} />
              <Route path="/eventseats" element={<EventSeats/>} />
              <Route path="/eventareas" element={<EventAreas/>} />
              <Route path="/tickets" element={<Tickets/>} />
              <Route path="account/personalaccount" element={<Account roles={roles}/>} />
            </Routes>
          </main>
        </div>
        <footer className="border-top footer text-muted">
          <div className="container">
              &copy; 2022 - Playbill
          </div>
        </footer>
      </div>
    </BrowserRouter>
  );
}

export default App;
