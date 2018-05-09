import React, { Component } from "react";
import { translate } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import { Grid, Button, Typography, CircularProgress, Divider, TextField } from 'material-ui';
import { FormControl, FormHelperText } from 'material-ui/Form';
import { withStyles } from 'material-ui/styles';
import { AccountService } from '../services';
import Input, { InputLabel } from 'material-ui/Input';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
});

class ConfirmCode extends Component {
    constructor(props) {
        super(props);
        this.handleConfirmCode = this.handleConfirmCode.bind(this);
        this.state = {
            confirmationCode: '',
            isLoading: false
        };
    }

    handleConfirmCode() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        AccountService.confirm(self.state.confirmationCode).then(function () {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('accountIsConfirmed')
            });
            self.props.history.push('/');
        }).catch(function () {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('confirmationCodeIsNotValid')
            });
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('confirmCodeTitle')}</h4>
                        <i>{t('confirmCodeDescription')}</i>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{ display: "inline-block" }}>{t('confirmCode')}</h4>
                </div>
                <div className="body">
                    {self.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            {/* Login */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('login')}</InputLabel>
                                <Input name="login" value={self.state.confirmationCode} disabled={true} />
                            </FormControl>
                            <Button variant="raised" color="primary" onClick={this.handleConfirmCode}>{t('confirm')}</Button>
                        </div>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.setState({
            confirmationCode: self.props.match.params.id
        });
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(withStyles(styles)(ConfirmCode)));