﻿import React, { Component } from "react";
import { Route, Redirect } from 'react-router-dom';

import Layout from './layout';
import { Login, About, ConfirmActivationCode, OfficeDocuments, ViewOfficeDocument } from './components';
import { SessionService } from './services';

export const routes = (<Layout>
    <Route exact path='/' component={About} />
    <Route exact path='/about' component={About} />
    <Route exact path='/login' component={Login} />
    <Route exact path='/documents' component={OfficeDocuments} />
    <Route exact path='/documents/:id' component={ViewOfficeDocument} />
    <Route exact path="/confirm/:code" render={(props) => SessionService.isExpired() ? <Redirect to="/login" /> : <ConfirmActivationCode {...props} /> } />
</Layout>);