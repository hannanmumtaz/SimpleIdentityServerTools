import React, { Component } from "react";
import { NavLink } from "react-router-dom";
import { withRouter, Link } from 'react-router-dom';
import { translate } from 'react-i18next';
import { SessionService } from './services';
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
        this.disconnect = this.disconnect.bind(this);
        this.navigate = this.navigate.bind(this);
        this.refresh = this.refresh.bind(this);
        this.startCheckSession = this.startCheckSession.bind(this);
        this.stopCheckSession = this.stopCheckSession.bind(this);
        this.handleSnackbarClose = this.handleSnackbarClose.bind(this);
        this.displayMessage = this.displayMessage.bind(this);
        var pathName = this.props.location.pathname;
        this.state = {
            isLoggedIn: false,
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