import React, { Component } from "react";
import { NavLink } from "react-router-dom";
import { withRouter, Link } from 'react-router-dom';
import { translate } from 'react-i18next';
import { SessionService } from './services';
import Constants from './constants';
import AppDispatcher from './appDispatcher';

import { IconButton, Button , Drawer, Select, MenuItem, SwipeableDrawer, FormControl, Grid, CircularProgress, Snackbar, Divider, Avatar, Typography } from 'material-ui';
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
import Collapse from 'material-ui/transitions/Collapse';

const drawerWidth = 300;
const styles = theme => ({
  root: {
    display: 'flex'
  },
  body: {
    marginLeft: drawerWidth + "px"
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
  }
});

class Layout extends Component {
    constructor(props) {
        super(props);
        this._appDispatcher = null;
        this._sessionFrame = null;
        this._checkSessionInterval = null;
        this.disconnect = this.disconnect.bind(this);
        this.toggleValue = this.toggleValue.bind(this);
        this.navigate = this.navigate.bind(this);
        this.refresh = this.refresh.bind(this);
        this.startCheckSession = this.startCheckSession.bind(this);
        this.stopCheckSession = this.stopCheckSession.bind(this);
        this.handleSelection = this.handleSelection.bind(this);
        this.handleSnackbarClose = this.handleSnackbarClose.bind(this);
        this.displayMessage = this.displayMessage.bind(this);
        this.state = {
            isManageOpenidServerOpened: false,
            isManageAuthServersOpened: false,
            isScimOpened: false,
            isLoggedIn: false,
            isOauthDisplayed: false,
            isScimDisplayed: false,
            isAuthDisplayed: false,
            isDrawerDisplayed: false,
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
     * Disconnect the user.
     * @param {any} e
     */
    disconnect() {
        AppDispatcher.dispatch({
            actionName: Constants.events.USER_LOGGED_OUT
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
    * Refresh the user information.
    */
    refresh() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        var image = "/img/unknown.png";
        var givenName = t("unknown");
        var role = '';
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

            if (claims.role) {
                role = claims.role;
            }
        }

        self.setState({
            user: {
                name: givenName,
                picture: image,
                role: role
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

        var session = SessionService.getSession();
        if (!session || !session.sessionState) {
            return;
        }

        var evt = window.addEventListener("message", function(e) {
            if (e.data !== 'unchanged') {
                AppDispatcher.dispatch({
                    actionName: Constants.events.USER_LOGGED_OUT
                });
                self.props.history.push('/');
            }
        }, false);
        var originUrl = window.location.protocol + "//" + window.location.host;
        self._checkSessionInterval = setInterval(function() { 
            var session = SessionService.getSession();
            var message = Constants.clientId + " ";
            if (session) {
                message += session.sessionState;
            } else {
                session += "tmp";
            }

            var win = self._sessionFrame.contentWindow;
            // TODO : Externalize the client_id & openid url.
            win.postMessage(message, Constants.openIdUrl);
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
        return (
        <div>
            <Drawer docked={true} variant="permanent" anchor="left" classes={{ paper: classes.drawerPaper }}>                
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
                    {/* Home menu item */}
                    {(self.state.isLoggedIn && self.state.user && self.state.user.role !== 'administrator' && (
                        <ListItem button onClick={() => self.navigate('/home')}>
                            <ListItemText>{t('homeMenuItem')}</ListItemText>
                        </ListItem>
                    ))}
                    {/* Users menu item */}
                    {(self.state.isLoggedIn && self.state.user && self.state.user.role === 'administrator' && (
                        <ListItem button onClick={() => self.navigate('/users')}>
                            <ListItemText>{t('usersMenuItem')}</ListItemText>
                        </ListItem>
                    ))}
                    {/* Connect or disconnect */}
                    {(this.state.isLoggedIn ? (
                        <ListItem button onClick={() => self.disconnect()}>
                            <ListItemText>{t('disconnectMenuItem')}</ListItemText>
                        </ListItem>
                    ) : (                        
                        <ListItem button onClick={() => self.navigate('/login')}>
                            <ListItemText>{t('connectMenuItem')}</ListItemText>
                        </ListItem>
                    ))}
                </List>
            </Drawer>
            <section className={classes.body}>
                { /* Navigation */ }
                <nav className="navbar navbar-toggleable-md">
                        <a className="navbar-brand" href="#" id="uma-title">
                            <img src="/img/logo.png" width="100" />
                        {t('websiteTitle')}
                    </a>
                </nav>
                { /* Display component */}
                <section id="body">
                    {this.props.children}
                </section>
            </section>
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
                    self.props.history.push('/login');
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