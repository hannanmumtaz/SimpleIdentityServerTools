import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid, Button, Typography, CircularProgress } from 'material-ui';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import { withStyles } from 'material-ui/styles';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
});


class Home extends Component {
    constructor(props) {
        super(props);
        this.refresh = this.refresh.bind(this);
        this.requestAccess = this.requestAccess.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.state = {
            isAccountEnabled: false,
            isAccessRequested: false,
            isLoading: false
        };
    }

    refresh() {

    }

    requestAccess() {

    }

    handleChange(e) {
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('homeTitle')}</h4>
                        <i>{t('homeDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item">{t('websiteTitle')}</li>
                            <li className="breadcrumb-item">{t('home')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{ display: "inline-block" }}>{t('home')}</h4>
                </div>
                <div className="body">
                    {self.state.isLoading ? (<CircularProgress />) : (
                        <div>
                            {self.state.isAccountEnabled === false && self.state.isAccessRequested === false && (
                                <div>
                                    <FormControl fullWidth={true} className={classes.margin}>
                                        <InputLabel>{t('emailAddress')}</InputLabel>
                                        <Input name="email" onChange={self.handleChange} value={self.state.email} />
                                    </FormControl>
                                    <Button variant="raised" color="primary" onClick={this.requestAccess}>{t('requestAccess')}</Button>
                                </div>
                            )}
                            {self.state.isAccountEnabled === false && self.state.isAccessRequested === true && (
                                <Typography variant="caption">{t('accessHasBeenRequested')}</Typography>
                            )}
                            {self.state.isAccountEnabled === true && (
                                <Typography variant="caption">{t('yourAccountIsEnabled')}</Typography>
                            )}
                        </div>
                    )}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {

    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(Home));