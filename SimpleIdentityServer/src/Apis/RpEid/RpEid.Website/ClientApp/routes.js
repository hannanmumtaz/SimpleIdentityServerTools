import React, { Component } from "react";
import { Route, Redirect } from 'react-router-dom';
import { SessionService } from './services';

import Layout from './layout';
import { Login, Home, Users, Profile, About, ConfirmCode } from './components';

export const routes = (<Layout>
    <Route exact path='/' component={About} />
    <Route exact path='/home' component={Home} />
    <Route exact path='/login' component={Login} />
    <Route exact path='/users' component={Users} />
    <Route exact path='/profile' component={Profile} />
    <Route exact path='/confirm/:id?' component={ConfirmCode} />
</Layout>);