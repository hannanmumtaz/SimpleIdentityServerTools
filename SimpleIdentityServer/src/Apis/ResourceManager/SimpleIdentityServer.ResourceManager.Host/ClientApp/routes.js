import React, { Component } from "react";
import { Route, Redirect } from 'react-router-dom';
import { SessionService } from './services';

import Layout from './layout';
import { Login, About, Logs, Resources, ViewAggregate, ViewLog,
 OAuthClients, OpenidClients, OAuthScopes, OpenidScopes, ResourceOwners, ViewResource, ViewClient, ViewScope,
 AddScope, Dashboard, ViewUser, Claims, ViewClaim, ScimSchemas, ScimResources, ViewScimSchema, ViewScimAttribute,
 ViewScimResource, Settings } from './components';

export const routes = (<Layout>
    <Route exact path='/' component={About} />
    <Route exact path='/about' component={About} />
    <Route exact path='/login' component={Login} />
    { !process.env.IS_LOG_DISABLED && (<Route exact path='/logs/:action?/:subaction?' component={Logs} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/dashboard' exact path='/dashboard' component={Dashboard} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/authclients' exact path='/authclients' component={OAuthClients} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/openidclients' exact path='/openidclients' component={OpenidClients} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/authscopes' exact path='/authscopes' component={OAuthScopes} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/openidscopes' exact path='/openidscopes' component={OpenidScopes} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/resourceowners' exact path='/resourceowners' component={ResourceOwners} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/claims' exact path='/claims' component={Claims} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/viewClient/:type/:id/:action?' exact path='/viewClient/:type/:id/:action?' component={ViewClient} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/viewScope/:type/:id' exact path='/viewScope/:type/:id' component={ViewScope} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/addScope/:type' exact path='/addScope/:type' component={AddScope} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/viewUser/:id' exact path='/viewUser/:id' component={ViewUser} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/viewClaim/:id' exact path='/viewClaim/:id' component={ViewClaim} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scimSchemas' exact path='/scimSchemas' component={ScimSchemas} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scimSchemas/:id' exact path='/scimSchemas/:id' component={ViewScimSchema} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scimSchemas/:id/:attr' exact path='/scimSchemas/:id/:attr' component={ViewScimAttribute} />)}    
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scimResources' exact path='/scimResources' component={ScimResources} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scimResources/:type/:id' exact path='/scimResources/:type/:id' component={ViewScimResource} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/settings' exact path='/settings' component={Settings} />)}
    { !process.env.IS_RESOURCES_DISABLED && (<Route exact path='/resources/:action?' component={Resources} />)}
    { !process.env.IS_RESOURCES_DISABLED && (<Route exact path='/resource/:id' component={ViewResource} />)}
    { !process.env.IS_LOG_DISABLED && (<Route exact path="/viewaggregate/:id" component={ViewAggregate} /> )}
    { !process.env.IS_LOG_DISABLED && (<Route exact path="/viewlog/:id" component={ViewLog} /> )}
</Layout>);