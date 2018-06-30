﻿import React, { Component } from "react";
import { Route, Redirect } from 'react-router-dom';
import { SessionService } from './services';

import Layout from './layout';
import { Login, About, Logs, Resources, ViewAggregate, ViewLog,
 Clients, Scopes, ResourceOwners, ViewResource, ViewClient, ViewScope,
 AddScope, Dashboard, ViewResourceOwner, Claims, ViewClaim, ScimSchemas, ScimResources, ViewScimSchema, ViewScimAttribute,
 ViewScimResource, Settings, ViewSetting, Connectors, TwoFactors } from './components';

export const routes = (<Layout>
    <Route exact path='/' component={About} />
    <Route exact path='/about' component={About} />
    <Route exact path='/login' component={Login} />
    { !process.env.IS_LOG_DISABLED && (<Route exact path='/logs/:action?/:subaction?' component={Logs} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/dashboard' exact path='/dashboard' component={Dashboard} />) }

    { /* OPENID */ }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/resourceowners' exact path='/resourceowners' component={ResourceOwners} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/resourceowners/:id' exact path='/resourceowners/:id' component={ViewResourceOwner} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/claims' exact path='/claims' component={Claims} />) }
    { /* OPENID + OAUTH */}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/:type/clients' exact path='/:type/clients' component={Clients} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/:type/clients/:id/:action?' exact path='/:type/clients/:id/:action?' component={ViewClient} />)}    
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/:type/scopes' exact path='/:type/scopes' component={Scopes} />) }
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/:type/scopes/:id' exact path='/:type/scopes/:id' component={ViewScope} />)}
    { /* SCIM */}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scim/schemas' exact path='/scim/schemas' component={ScimSchemas} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scim/schemas/:id' exact path='/scim/schemas/:id' component={ViewScimSchema} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scim/schemas/:id/:attr' exact path='/scim/schemas/:id/:attr' component={ViewScimAttribute} />)}    
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scim/resources' exact path='/scim/resources' component={ScimResources} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/scim/resources/:type/:id' exact path='/scim/resources/:type/:id' component={ViewScimResource} />)}

    { !process.env.IS_MANAGE_DISABLED && (<Route key='/viewClaim/:id' exact path='/viewClaim/:id' component={ViewClaim} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/settings' exact path='/settings' component={Settings} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/settings/:id' exact path='/settings/:id' component={ViewSetting} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/twofactors' exact path='/twofactors' component={TwoFactors} />)}
    { !process.env.IS_MANAGE_DISABLED && (<Route key='/connectors' exact path='/connectors' component={Connectors} />)}
    { !process.env.IS_RESOURCES_DISABLED && (<Route exact path='/resources/:action?' component={Resources} />)}
    { !process.env.IS_RESOURCES_DISABLED && (<Route exact path='/resource/:id' component={ViewResource} />)}
    { !process.env.IS_LOG_DISABLED && (<Route exact path="/viewaggregate/:id" component={ViewAggregate} /> )}
    { !process.env.IS_LOG_DISABLED && (<Route exact path="/viewlog/:id" component={ViewLog} /> )}
</Layout>);