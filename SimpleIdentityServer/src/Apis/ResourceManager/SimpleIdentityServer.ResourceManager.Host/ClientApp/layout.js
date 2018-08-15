import React, { Component } from "react";
import { NavLink } from "react-router-dom";
import { withRouter, Link } from 'react-router-dom';
import { translate } from 'react-i18next';
import { SessionService, EndpointService, ProfileService } from './services';
import Constants from './constants';
import AppDispatcher from './appDispatcher';

import { IconButton, Button , Drawer, Select, Menu, MenuItem, SwipeableDrawer, FormControl, Grid, CircularProgress, Snackbar, Divider, Avatar, Typography, Hidden } from 'material-ui';
import  List, { ListItem, ListItemText, ListItemIcon } from 'material-ui/List';
import { InputLabel } from 'material-ui/Input';
import { withStyles } from 'material-ui/styles';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import Settings from '@material-ui/icons/Settings';
import Face from '@material-ui/icons/Face';
import Language from '@material-ui/icons/Language';
import Label from '@material-ui/icons/Label';
import Lock from '@material-ui/icons/Lock';
import Assignment from '@material-ui/icons/Assignment';
import MenuIcon from '@material-ui/icons/Menu';
import FilterList from '@material-ui/icons/FilterList';
import Collapse from 'material-ui/transitions/Collapse';

const drawerWidth = 300;
const styles = theme => ({
  root: {
    display: 'flex', 
    overflow: 'hidden',
    position: 'relative',
    width: '100%',
    flexGrow: 1,
  },
  body: {
    flexGrow: 1,
    [theme.breakpoints.up('md')]: {
      marginLeft: drawerWidth + "px"
    }
  },
  drawerPaper: {
    width: drawerWidth
  },
  formControl: {
    margin: theme.spacing.unit,
    minWidth: 120,
  },
  nested: {
    paddingLeft: theme.spacing.unit * 4,
  },
  avatar: {
    width: 120,
    height: 120
  },
  navIconHide: {
    [theme.breakpoints.up('md')]: {
      display: 'none',
    },
  }
});

class Layout extends Component {
    constructor(props) {
        super(props);
        this._appDispatcher = null;
        this._sessionFrame = null;
        this._checkSessionInterval = null;
        this.handleDrawerToggle = this.handleDrawerToggle.bind(this);
        this.disconnect = this.disconnect.bind(this);
        this.toggleValue = this.toggleValue.bind(this);
        this.navigate = this.navigate.bind(this);
        this.refresh = this.refresh.bind(this);
        this.startCheckSession = this.startCheckSession.bind(this);
        this.stopCheckSession = this.stopCheckSession.bind(this);
        this.handleSelection = this.handleSelection.bind(this);
        this.handleSaveChanges = this.handleSaveChanges.bind(this);
        this.handleSnackbarClose = this.handleSnackbarClose.bind(this);
        this.displayMessage = this.displayMessage.bind(this);
        var pathName = this.props.location.pathname;
        this.state = {
            isManageOpenidServerOpened: pathName.indexOf('/claims') !== -1 || pathName.indexOf('/users') !== -1 || pathName.indexOf('/openid') !== -1,
            isManageAuthServersOpened: pathName.indexOf('/auth') !== -1 ||  pathName.indexOf('/resources') !== -1,
            isScimOpened: pathName.indexOf('/scim') !== -1,
            isSettingsOpened: pathName.indexOf('/units') !== -1 || pathName.indexOf('/connectors') !== -1 || pathName.indexOf('/twofactors') !== -1,
            isLoggedIn: false,
            isOauthDisplayed: false,
            isScimDisplayed: false,
            isAuthDisplayed: false,
            isDrawerDisplayed: false,
            mobileOpen: false,
            openidEndpoints: [],
            authEndpoints: [],
            scimEndpoints: [],
            selectedOpenid: null,
            selectedAuth: null,
            selectedScim: null,
            isLoading: false,
            isSnackbarOpened: false,
            snackbarMessage: '',
            user: {}
        };
    }

    /**
    * Toggle the drawer.
    */
    handleDrawerToggle() {
        this.setState({ mobileOpen: !this.state.mobileOpen });
    }

    /**
     * Disconnect the user.
     * @param {any} e
     */
    disconnect() {
        var url = Constants.openidUrl + "/end_session?post_logout_redirect_uri="+Constants.adminuiUrl+"/end_session&id_token_hint="+ SessionService.getSession().id_token;
        var w = window.open(url, '_blank');
        var interval = setInterval(function() {
            if (w.closed) {
                clearInterval(interval);
                return;
            }

            var href = w.location.href;
            if (href === Constants.adminuiUrl + "/end_session") {                
                clearInterval(interval);
                w.close();
            }
        });
    }

    toggleValue(menu) {    
        this.setState({
            [menu]: !this.state[menu]
        });
    }

    navigate(href) {
        this.props.history.push(href);
    }

    /**
    * Refresh the settings menu.
    */
    refresh() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });

        EndpointService.getAll().then(function(endpoints) {
            var authEndpoints = endpoints.filter(function(endpoint) { return endpoint.type === 0; });
            var openidEndpoints = endpoints.filter(function(endpoint) { return endpoint.type === 1; });
            var scimEndpoints = endpoints.filter(function(endpoint) { return endpoint.type === 2; });
            ProfileService.getMineProfile().then(function(profile) {
                var selectedOpenid = openidEndpoints.filter(function(endpoint) { return endpoint.url === profile['openid_url'] })[0];
                self.setState({
                    authEndpoints: authEndpoints,
                    openidEndpoints: openidEndpoints,
                    scimEndpoints: scimEndpoints,
                    selectedOpenid: profile['openid_url'],
                    selectedAuth: profile['auth_url'],
                    selectedScim: profile['scim_url'],
                    isLoading: false
                });
                AppDispatcher.dispatch({
                    actionName: Constants.events.SESSION_CREATED,
                    data: {
                        selectedOpenid: openidEndpoints.filter(function(endpoint) { return endpoint.url === profile['openid_url'] })[0],
                        selectedAuth: authEndpoints.filter(function(endpoint) { return endpoint.url === profile['auth_url'] })[0],
                        selectedScim: scimEndpoints.filter(function(endpoint) { return endpoint.url === profile['scim_url'] })[0]
                    }
                });
            }).catch(function() {
                var selectedOpenid = null;
                var selectedAuth = null;
                var selectedScim = null;
                if (authEndpoints.length > 0) {
                    selectedAuth = authEndpoints[0];
                }

                if (openidEndpoints.length > 0) {
                    selectedOpenid = openidEndpoints[0];
                }

                if (scimEndpoints.length > 0) {
                    selectedScim = scimEndpoints[0];
                }

                self.setState({
                    authEndpoints: authEndpoints,
                    openidEndpoints: openidEndpoints,
                    scimEndpoints: scimEndpoints,
                    selectedOpenid: selectedOpenid.url,
                    selectedAuth: selectedAuth.url,
                    selectedScim: selectedScim.url,
                    isLoading: false
                });
                AppDispatcher.dispatch({
                    actionName: Constants.events.SESSION_CREATED,
                    data: {
                        selectedOpenid: selectedOpenid,
                        selectedAuth: selectedAuth,
                        selectedScim: selectedScim
                    }
                });
            });
        }).catch(function() {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('profileCannotBeRetrieved')
            });
        });

        var image = "/img/unknown.png";
        var givenName = t("unknown");
        var session = SessionService.getSession();
        if (session && session.id_token) {
            var idToken = session.id_token;
            var splitted = idToken.split('.');
            var claims = JSON.parse(window.atob(splitted[1]));
            if (claims.picture) {
                image = claims.picture;
            }

            if (claims.given_name) {
                givenName = claims.given_name;
            }
        }

        self.setState({
            user: {
                name: givenName,
                picture: image
            }
        });
    }

    /**
    * Start the check the session.
    */
    startCheckSession() {
        var self = this;
        if (self._checkSessionInterval) {
            return;
        }

        var evt = window.addEventListener("message", function (e) {
            if (e.data !== 'unchanged' && (typeof e.data === 'string')) {
                AppDispatcher.dispatch({
                    actionName: Constants.events.USER_LOGGED_OUT
                });
                self.props.history.push('/');
            }
        }, false);
        var originUrl = window.location.protocol + "//" + window.location.host;
        self._checkSessionInterval = setInterval(function() { 
            var session = SessionService.getSession();
            var message = "ResourceManagerClientId ";
            if (session) {
                message += session.sessionState;
            } else {
                session += "tmp";
            }
            
            var win = self._sessionFrame.contentWindow;
            win.postMessage(message, Constants.openidUrl);
        }, 3*1000);
    }

    /**
    * Stop to check the session.
    */
    stopCheckSession() {
        var self = this;
        if (!self._checkSessionInterval) {
            return;
        }

        clearInterval(self._checkSessionInterval);
        self._checkSessionInterval = null;
    }

    /**
    * Handle the selection.
    */
    handleSelection(e) {
        this.setState({
            [e.target.name]: e.target.value
        });
    }

    /**
    *   Save the profile.
    */
    handleSaveChanges() {
        var self = this;
        const { t } = self.props;
        var request = {
            openid_url: self.state.selectedOpenid,
            auth_url: self.state.selectedAuth,
            scim_url: self.state.selectedScim
        };
        self.setState({
            isLoading: true
        });
        ProfileService.updateMineProfile(request).then(function() {
            self.setState({
                isLoading: false,
                isDrawerDisplayed: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('profileSaved')
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.SESSION_UPDATED,
                data: request
            });
        }).catch(function() {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('profileCannotBeSaved')
            });
            self.setState({
                isLoading: false
            });
        });
    }

    /**
    * Snackbar is closed.
    */
    handleSnackbarClose(message) {
        this.setState({
            isSnackbarOpened: false
        });
    }

    /**
    * Display the message in the snackbar.
    */
    displayMessage(message) {
        this.setState({
            isSnackbarOpened: true,
            snackbarMessage: message
        });
    }

    render() {
        var self = this;
        const { t, classes } = this.props;
        var openidEndpoints = [];
        var authEndpoints = [];
        var scimEndpoints = [];
        if (self.state.openidEndpoints) {
            self.state.openidEndpoints.forEach(function(openidEndpoint) {
                openidEndpoints.push((<MenuItem key={openidEndpoint.name} value={openidEndpoint.url}>{openidEndpoint.description}</MenuItem>));
            });
        }

        if (self.state.authEndpoints) {
            self.state.authEndpoints.forEach(function(authEndpoint) {
                authEndpoints.push((<MenuItem key={authEndpoint.name} value={authEndpoint.url}>{authEndpoint.description}</MenuItem>));
            });
        }

        if (self.state.scimEndpoints) {
            self.state.scimEndpoints.forEach(function(scimEndpoint) {
                scimEndpoints.push((<MenuItem key={scimEndpoint.name} value={scimEndpoint.url}>{scimEndpoint.description}</MenuItem>))
            });
        }

        var pathName = self.props.location.pathname;
        var drawer = (
                <List>
                    {(self.state.isLoggedIn && (
                        <ListItem>
                            <div style={{ width: "100%", "textAlign": "center"}}>
                                <img src={self.state.user.picture} style={{"width": "80px", "height": "80px"}} className="img-circle img-thumbnail" />
                                <Typography variant="title">{self.state.user.name}</Typography>
                            </div>
                        </ListItem>
                    ))}
                    {(self.state.isLoggedIn && (
                        <Divider />
                    ))}
                    {/* About menu item */}
                    <MenuItem key="/about" selected={pathName === '/about' || pathName === '/'} onClick={() => self.navigate('/about')}>{t('aboutMenuItem')}</MenuItem>
                    {/* Dashboard menu item */}                        
                    {(self.state.isLoggedIn && (
                        <MenuItem key="/dashboard" selected={pathName === '/dashboard'} onClick={() => self.navigate('/dashboard')}>{t('dashboardMenuItem')}</MenuItem>
                    ))}
                    {/* Openid menu item */}                        
                    {(self.state.isLoggedIn && (
                        <MenuItem onClick={() => self.toggleValue('isManageOpenidServerOpened')}>
                            { this.state.isManageOpenidServerOpened ? (<ListItemIcon><ExpandLess /></ListItemIcon>) : (<ListItemIcon><ExpandMore /></ListItemIcon>) }
                            {t('manageOpenidServers')}
                        </MenuItem>
                    ))}
                    {(self.state.isLoggedIn && (
                        <Collapse in={this.state.isManageOpenidServerOpened}>
                            <List>
                                <MenuItem key='/claims' selected={pathName.indexOf('/claims') !== -1} className={classes.nested} onClick={() => self.navigate('/claims')}>
                                    <ListItemIcon><Assignment /></ListItemIcon>
                                    {t('claims')}
                                </MenuItem>
                                <MenuItem key='/users' selected={pathName.indexOf('/users') !== -1} className={classes.nested} onClick={() => self.navigate('/users')}>
                                    <ListItemIcon><Face /></ListItemIcon>
                                    {t('resourceOwners')}
                                </MenuItem>
                                <MenuItem key='/openid/clients' selected={pathName.indexOf('/openid/clients') !== -1} className={classes.nested} onClick={() => self.navigate('/openid/clients')}>
                                    <ListItemIcon><Language /></ListItemIcon>
                                    {t('openidclients')}
                                </MenuItem>
                                <MenuItem key='/openid/scopes' selected={pathName.indexOf('/openid/scopes') !== -1} className={classes.nested} onClick={() => self.navigate('/openid/scopes')}>
                                    <ListItemIcon><Label /></ListItemIcon>
                                    {t('openidScopes')}
                                </MenuItem>
                                <MenuItem key='/openid/accountfilters' selected={pathName.indexOf('/openid/accountfilters') !== -1} className={classes.nested} onClick={() => self.navigate('/openid/accountfilters')}>
                                    <ListItemIcon><FilterList /></ListItemIcon>
                                    {t('accountFilters')}
                                </MenuItem> 
                            </List>
                        </Collapse>   
                    ))}       
                    {/* Authorization server */}
                    {(this.state.isLoggedIn && (
                        <MenuItem onClick={() => self.toggleValue('isManageAuthServersOpened')}>
                            { this.state.isManageAuthServersOpened ? (<ListItemIcon><ExpandLess /></ListItemIcon>) : (<ListItemIcon><ExpandMore /></ListItemIcon>) }
                            {t('manageAuthServers')}
                        </MenuItem>
                    ))}
                    {(this.state.isLoggedIn && (
                        <Collapse in={this.state.isManageAuthServersOpened}>
                            <List>
                                <MenuItem key='/authclients' selected={pathName.indexOf('/auth/clients') !== -1} className={classes.nested} onClick={() => self.navigate('/auth/clients')}>
                                    <ListItemIcon><Language /></ListItemIcon>
                                    {t('oauthClients')}
                                </MenuItem>
                                <MenuItem key='/authScopes' selected={pathName.indexOf('/auth/scopes') !== -1} className={classes.nested} onClick={() => self.navigate('/auth/scopes')}>
                                    <ListItemIcon><Label /></ListItemIcon>
                                    {t('authscopes')}
                                </MenuItem>
                                <MenuItem key='/resources' selected={pathName.indexOf('/resources') !== -1} className={classes.nested} onClick={() => self.navigate('/resources')}>
                                    <ListItemIcon><Lock /></ListItemIcon>
                                    {t('resources')}
                                </MenuItem>
                            </List>
                        </Collapse>
                    ))}
                    {/* SCIM server */}
                    {(this.state.isLoggedIn && process.env.IS_COMMERCIAL && (
                        <MenuItem onClick={() => self.toggleValue('isScimOpened')}>
                            { this.state.isScimOpened ? (<ListItemIcon><ExpandLess /></ListItemIcon>) : (<ListItemIcon><ExpandMore /></ListItemIcon>) }
                            {t('manageScimServers')}
                        </MenuItem>
                    ))}
                    {this.state.isLoggedIn && process.env.IS_COMMERCIAL && (
                        <Collapse in={this.state.isScimOpened}>
                            <List>
                                <MenuItem key='/scim/schemas' selected={pathName.indexOf('/scim/schemas') !== -1} className={classes.nested} onClick={() => self.navigate('/scim/schemas')}>
                                    {t('scimSchemas')}
                                </MenuItem>
                                <MenuItem key='/scim/resources' selected={pathName.indexOf('/scim/resources') !== -1} className={classes.nested} onClick={() => self.navigate('/scim/resources')}>
                                    {t('scimResources')}
                                </MenuItem>
                            </List>
                        </Collapse>
                    )}
                    {/* SETTINGS */}
                    {/*
                    {(this.state.isLoggedIn && process.env.IS_COMMERCIAL && (
                        <MenuItem onClick={() => self.toggleValue('isSettingsOpened')}>
                            { this.state.isSettingsOpened ? (<ListItemIcon><ExpandLess /></ListItemIcon>) : (<ListItemIcon><ExpandMore /></ListItemIcon>) }
                            {t('settings')}
                        </MenuItem>
                    ))}
                    {(this.state.isLoggedIn && process.env.IS_COMMERCIAL && (
                        <Collapse in={this.state.isSettingsOpened}>
                            <List>
                                <MenuItem key="/units" selected={pathName.indexOf('/units') !== -1} className={classes.nested} onClick={() => self.navigate('/units')}>{t('unitsModuleItem')}</MenuItem>
                                <MenuItem key="/connectors" selected={pathName.indexOf('/connectors') !== -1} className={classes.nested} onClick={() => self.navigate('/connectors')}>{t('connectorsMenuItem')}</MenuItem>
                                <MenuItem key="/twofactors" selected={pathName.indexOf('/twofactors') !== -1} className={classes.nested} onClick={() => self.navigate('/twofactors')}>{t('twofactorsMenuItem')}</MenuItem>
                            </List>
                        </Collapse>
                    ))}
                    */}
                    {/* Logs */}         
                    {this.state.isLoggedIn && process.env.IS_COMMERCIAL && (
                       <MenuItem key='logs' selected={pathName.indexOf('/logs') !== -1} onClick={() => self.navigate('/logs')}>
                            {t('logsMenuItem')}
                        </MenuItem>
                    )}
                    {/* Connect or disconnect */}
                    {(this.state.isLoggedIn ? (
                        <MenuItem onClick={() => self.disconnect()}>
                            {t('disconnectMenuItem')}
                        </MenuItem>
                    ) : (                        
                        <MenuItem onClick={() => self.navigate('/login')}>
                            {t('connectMenuItem')}
                        </MenuItem>
                    ))}
                </List>
        );
        return (
        <div className={classes.root}>
            <SwipeableDrawer open={self.state.isDrawerDisplayed} anchor="right" onClose={ () => self.setState({ isDrawerDisplayed: false }) } onOpen={ () => self.setState({ isDrawerDisplayed: true }) }>
                {self.state.isLoading ? (<CircularProgress />) : (
                    <div style={{padding: "20px"}}>
                        <div>
                            {openidEndpoints.length > 0 && (
                                <div>
                                    <FormControl fullWidth={true} className={classes.formControl}>
                                        <InputLabel>{t('selectedPreferredOpenIdProvider')}</InputLabel>
                                        <Select value={self.state.selectedOpenid} name='selectedOpenid' onChange={self.handleSelection}>
                                            {openidEndpoints}
                                        </Select>
                                    </FormControl>
                                </div>
                            )}
                            {authEndpoints.length > 0 && (
                                <div>
                                    <FormControl fullWidth={true} className={classes.formControl}>
                                        <InputLabel>{t('selectPreferredAuthorizationServer')}</InputLabel>
                                        <Select  value={self.state.selectedAuth} name='selectedAuth' onChange={self.handleSelection}>
                                            {authEndpoints}
                                        </Select>
                                    </FormControl>
                                </div>
                            )}
                            {scimEndpoints.length > 0 && (
                                <div>
                                    <FormControl fullWidth={true} className={classes.formControl}>
                                        <InputLabel>{t('selectPreferredScimServer')}</InputLabel>                            
                                        <Select value={self.state.selectedScim} name='selectedScim' onChange={self.handleSelection}>
                                            {scimEndpoints}
                                        </Select>
                                    </FormControl>
                                </div>
                            )}
                            <Button  variant="raised" color="primary" onClick={self.handleSaveChanges}>{t('saveChanges')}</Button>
                        </div>
                    </div>
                )}
            </SwipeableDrawer>
            <Hidden mdUp>
                <Drawer variant="temporary" anchor="left" open={self.state.mobileOpen} onClose={self.handleDrawerToggle}>
                    {drawer}
                </Drawer>
            </Hidden>
            <Hidden smDown implementation="css">
                <Drawer docked={true} variant="permanent" anchor="left" classes={{ paper: classes.drawerPaper }}>                
                    {drawer}
                </Drawer>
            </Hidden>
            <section className={classes.body}>
                { /* Navigation */ }
                <nav className="navbar navbar-toggleable-md">
                    <IconButton onClick={self.handleDrawerToggle} className={classes.navIconHide}>
                        <MenuIcon />
                    </IconButton>
                    <a className="navbar-brand" href="#" id="uma-title">{t('websiteTitle')}</a>
                    <ul className="navbar-nav mr-auto">
                    </ul>
                    {this.state.isLoggedIn && (<IconButton  onClick={() => self.toggleValue('isDrawerDisplayed')}><Settings button  /></IconButton>)}
                </nav>
                { /* Display component */}
                <section id="body">
                    {this.props.children}
                </section>
            </section>
            { this.state.isLoggedIn && (<div>
                    <iframe ref={(elt) => { self._sessionFrame = elt; self.startCheckSession(); }} id="session-frame" src={Constants.openidUrl + "/check_session"} style={{display: "none"}} /> 
                </div>
            )}
            <Snackbar anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }} open={self.state.isSnackbarOpened} onClose={self.handleSnackbarClose} message={<span>{self.state.snackbarMessage}</span>} />
        </div>);
    }

    componentDidMount() {
        var self = this;
        self._appDispatcher = AppDispatcher.register(function (payload) {
            switch (payload.actionName) {
                case Constants.events.USER_LOGGED_IN:
                    self.setState({
                        isLoggedIn: true
                    });
                    self.refresh();
                    break;
                case Constants.events.USER_LOGGED_OUT:
                    self.setState({
                        isLoggedIn: false
                    });
                    SessionService.remove();
                    break;
                case Constants.events.DISPLAY_MESSAGE:
                    self.displayMessage(payload.data);
                    break;
            }
        });

        var isLoggedIn = !SessionService.isExpired();
        if (isLoggedIn) {
            AppDispatcher.dispatch({
                actionName: Constants.events.USER_LOGGED_IN
            });
        }        
    }

    componentWillUnmount() {
        AppDispatcher.unregister(this._appDispatcher);
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(withStyles(styles)(Layout)));