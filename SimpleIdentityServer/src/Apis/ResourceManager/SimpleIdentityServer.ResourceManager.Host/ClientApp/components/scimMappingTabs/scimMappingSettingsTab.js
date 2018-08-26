import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid, Checkbox } from 'material-ui';
import { NavLink } from 'react-router-dom';
import { CircularProgress, Button, Select } from 'material-ui';
import { withStyles } from 'material-ui/styles';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import { ScimService } from '../../services';
import AppDispatcher from '../../appDispatcher';
import Constants from '../../constants';
import { SessionStore } from '../../stores';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit
    }
});

class ScimMappingSettingsTab extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.toggleProperty = this.toggleProperty.bind(this);
        this.state = {
            isLoading: true,
            adConfiguration: {}
        };
    }

    refreshData() {
        var self = this;
        const { t } = self.props;
        self.setState({
            isLoading: true
        });
        ScimService.getAdConfiguration().then(function (conf) {
            self.setState({
                adConfiguration: conf,
                isLoading: false
            });
        }).catch(function () {
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: t('cannotGetAdConfiguration')
            });
            self.setState({
                isLoading: false,
                adConfiguration: { }
            });
        });
    }

    /**
     *  Toggle the property
     * */
    toggleProperty() {
        var self = this;
        var adConfiguration = self.state.adConfiguration;
        adConfiguration['is_enabled'] = !adConfiguration['is_enabled'];
        self.setState({
            adConfiguration: adConfiguration
        });
    }

	/**
	* Change the property.
	*/
    handleChangeProperty(e) {
        var self = this;
        var adConfiguration = self.state.adConfiguration;
        adConfiguration[e.target.name] = e.target.value;
        self.setState({
            adConfiguration: adConfiguration
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        return (
            <div>
                {self.state.isLoading ? (<CircularProgress />) : (
                    <Grid container>
                        <Grid item md={6}>
                            {/* Is enabled */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <Checkbox color="primary" checked={self.state.adConfiguration.is_enabled} name="is_enabled" onChange={self.toggleProperty} />
                                <FormHelperText>{t('isScimMappingEnabledDescription')}</FormHelperText>
                            </FormControl>
                            {/* IP ADDR*/ }
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('adIpAddr')}</InputLabel>
                                <Input value={self.state.adConfiguration.ip_adr} name="ip_adr" onChange={self.handleChangeProperty} />
                                <FormHelperText>{t('adIpAddrDescription')}</FormHelperText>
                            </FormControl>
                            {/* Port */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('adPort')}</InputLabel>
                                <Input type="number" value={self.state.adConfiguration.port} name="port" onChange={self.handleChangeProperty} />
                                <FormHelperText>{t('adPortDescription')}</FormHelperText>
                            </FormControl>
                            {/* Username */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('adUserName')}</InputLabel>
                                <Input value={self.state.adConfiguration.username} name="username" onChange={self.handleChangeProperty} />
                                <FormHelperText>{t('adUserNameDescription')}</FormHelperText>
                            </FormControl>
                            {/* Password */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('adPassword')}</InputLabel>
                                <Input value={self.state.adConfiguration.password} name="password" onChange={self.handleChangeProperty} />
                                <FormHelperText>{t('adPasswordDescription')}</FormHelperText>
                            </FormControl>
                            {/* DistinguishedName */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('adDistinguishedName')}</InputLabel>
                                <Input value={self.state.adConfiguration.distinguished_name} name="distinguished_name" onChange={self.handleChangeProperty} />
                                <FormHelperText>{t('adDistinguishedNameDescription')}</FormHelperText>
                            </FormControl>
                            <Button variant="raised" color="primary">{t('saveAdConfiguration')}</Button>
                        </Grid>
                        <Grid item md={6}>
                            {/* Select SCHEMA identifier */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('schemaId')}</InputLabel>
                                <FormHelperText>{t('schemaIdDescription')}</FormHelperText>
                            </FormControl>
                            {/* Filter user */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('userFilter')}</InputLabel>
                                <Input />
                                <FormHelperText>{t('userFilterDescription')}</FormHelperText>
                            </FormControl>
                            {/* Filter user */}
                            <FormControl fullWidth={true} className={classes.margin}>
                                <InputLabel>{t('userFilterClass')}</InputLabel>
                                <Input />
                                <FormHelperText>{t('userFilterClassDescription')}</FormHelperText>
                            </FormControl>
                            <Button variant="raised" color="primary">{t('addSchemaConfiguration')}</Button>
                        </Grid>
                    </Grid>
                )} 
            </div>
        );
    }

    componentDidMount() {
        var self = this;
        SessionStore.addChangeListener(function () {
            self.refreshData();
        });

        if (SessionStore.getSession().selectedOpenid) {
            self.refreshData();
        }        
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ScimMappingSettingsTab));