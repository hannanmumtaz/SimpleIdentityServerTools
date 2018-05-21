import React, { Component } from "react";
import { Route, Redirect } from 'react-router-dom';
import { SessionService } from './services';

import Layout from './layout';
import { Home, Login, Users, ConfirmCode, Profile } from './components';

var isConnected = function() {
    var session = SessionService.getSession();
    return session && session.id_token;
};

var isAdministrator = function() {
    var session = SessionService.getSession();
    var role = "";
    if (session && session.id_token) {
        var idToken = session.id_token;
        var splitted = idToken.split('.');
        var claims = JSON.parse(window.atob(splitted[1]));
        if (claims.role) {
            role = claims.role;
        }
    }

    return role === "administrator";
};

export const routes = (<Layout>
    <Route exact path='/' component={Home} />
    <Route exact path='/login' render={() => (!isConnected() ? (<Login />) : (<Redirect to="/"/>))}/>
    <Route exact path='/users' render={() => (isConnected() && isAdministrator() ? (<Users />) : (<Redirect to="/"/>))}/>
    <Route exact path='/profile' render={() => (isConnected() && !isAdministrator() ? (<Profile />) : (<Redirect to="/"/>))}/>
    <Route exact path='/confirm/:id?' render={() => (isConnected() && !isAdministrator() ? (<ConfirmCode />) : (<Redirect to="/"/>))}/>
</Layout>);