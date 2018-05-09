import React, { Component } from "react";
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { SessionService, WebsiteService } from '../services';
import { withRouter } from 'react-router-dom';
import { translate } from 'react-i18next';
import { withStyles } from 'material-ui/styles';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';

import { TextField, Button, CircularProgress, Divider } from 'material-ui';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
});

class Login extends Component {
    constructor(props) {
        super(props);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.localAuthenticate = this.localAuthenticate.bind(this);
        this.externalAuthenticate = this.externalAuthenticate.bind(this);
        this.state = {
            login           : '',
            password        : '',
            isLoading       : false
        };
    }
    handleInputChange(e) {
        const target = e.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    parseJwt(token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        return JSON.parse(window.atob(base64));
    }

    /**
     * Local authentication.
     */
    localAuthenticate() {
        const { t } = this.props;
        var self = this;
        self.setState({
            isLoading: true
        });
        WebsiteService.authenticate(self.state.login, self.state.password).then(function (result) {
            self.setState({
                isLoading: false
            });

            if (result['access_token'] === '' || result['access_token'] === null) {
                AppDispatcher.dispatch({
                    actionName: Constants.events.DISPLAY_MESSAGE,
                    data: t('credentialsAreNotCorrect')
                });
                return;
            }

            var session = {
                token: result['access_token'],
                id_token: result['id_token']
            };
            SessionService.setSession(session);
            AppDispatcher.dispatch({
                actionName: Constants.events.USER_LOGGED_IN
            });
            self.props.history.push('/');
        }).catch(function (e) {
            console.log(e);
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('credentialsAreNotCorrect')
            });
        });
    }

    /**
    * External authentication.
    */
    externalAuthenticate() {
        var getParameterByName = function (name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        };        
        const {t} = this.props;
        var self = this;
        self.setState({
            isLoading: true
        });
        // TODO : Resolve this url.
        var url = Constants.openIdUrl + "/authorization?scope=openid role profile&state=75BCNvRlEGHpQRCT&redirect_uri=" + Constants.baseUrl + "/callback&response_type=id_token token&client_id=" + Constants.clientId +"&nonce=nonce&response_mode=query";
        var w = window.open(url, 'targetWindow', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=400,height=400');        
        var interval = setInterval(function () {
            if (w.closed) {
                clearInterval(interval);
                return;
            }

            var href = w.location.href;
            var idToken = getParameterByName('id_token', href);
            var accessToken = getParameterByName('access_token', href);
            var sessionState = getParameterByName('session_state', href);
            if (!idToken && !accessToken && !sessionState) {
                return;
            }

            sessionState = sessionState.replace(' ', '+');
            var payload = self.parseJwt(idToken);
            var session = {
                token: accessToken,
                id_token: idToken,
                sessionState: sessionState
            };
            SessionService.setSession(session);
            AppDispatcher.dispatch({
                actionName: Constants.events.USER_LOGGED_IN
            });
            self.props.history.push('/');
            self.setState({
                isLoading: false
            });
            clearInterval(interval);
            w.close();
        });
    }

    render() {
        const { t, classes } = this.props;
        var self = this;
        return (<div className="block">
            <div className="block-header">
                <h4>{t('loginTitle')}</h4>
            </div>
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-12">
                        <div className="card">
                            <div className="header">
                                <h4 style={{ display: "inline-block" }}>{t('authenticate')}</h4>
                            </div>
                            <div className="body">
                                { self.state.isLoading ? (<CircularProgress />) : (
                                    <div>
                                        <form style={{margin: "0px 0px 10px 0px"}} onSubmit={(e) => { e.preventDefault(); self.localAuthenticate(); }}>
                                            {/* Login */}
                                            <FormControl fullWidth={true} className={classes.margin}>
                                                <InputLabel>{t('login')}</InputLabel>
                                                <Input name="login" onChange={self.handleInputChange} value={self.state.login} />
                                            </FormControl>
                                            {/* Password */}
                                            <FormControl fullWidth={true} className={classes.margin}>
                                                <InputLabel>{t('password')}</InputLabel>
                                                <Input type="password" name="password" onChange={self.handleInputChange} value={self.state.password} />
                                            </FormControl>
                                            <Button variant="raised" color="primary" onClick={this.localAuthenticate}>{t('authenticate')}</Button>
                                        </form>
                                        <Divider />
                                        <Button style={{ margin: "10px 0px 0px 0px" }} variant="raised" color="primary" onClick={this.externalAuthenticate}>{t('eidConnect')}</Button>
                                    </div>
                                 )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(withStyles(styles)(Login)));