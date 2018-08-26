import React, { Component } from "react";
import { translate } from 'react-i18next';
import { ScimService } from '../services';
import { withStyles } from 'material-ui/styles';
import { NavLink, Link } from 'react-router-dom';
import { CircularProgress, IconButton, Grid } from 'material-ui';
import Tabs, { Tab } from 'material-ui/Tabs';
import { SessionStore } from '../stores';
import { ScimMappingSettingsTab, ScimMappingsTab } from './scimMappingTabs';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit
    }
});

class ScimMappings extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tabIndex: 0
        };
    }

    /**
    * Change tab.
    */
    handleTabChange(evt, val) {
        this.setState({
            tabIndex: val
        });
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scimMappingsTitle')}</h4>
                        <i>{t('scimMappingsTitleShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimMappingsTitle')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{ display: "inline-block" }}>{t('scimMappingsInformation')}</h4>
                </div>
                <div className="body">
                    <Tabs indicatorColor="primary" value={self.state.tabIndex} onChange={self.handleTabChange}>
                        <Tab label={t('listScimMappings')} component={NavLink} to="/scim/mappings" />
                        <Tab label={t('scimMappingSettings')} component={NavLink} to="/scim/mappings/settings" />
                    </Tabs>
                    {self.state.tabIndex === 0 && (<ScimMappingsTab />)}
                    {self.state.tabIndex === 1 && (<ScimMappingSettingsTab  />)}
                </div>
            </div>
        </div>);
    }

    componentDidMount() {
        var self = this;
        var tabIndex = 0;
        if (self.props.match.params.action === 'settings') {
            tabIndex = 1;
        }

        self.setState({
            tabIndex: tabIndex
        });
    }
}

export default translate('common', { wait: process && !process.release })(withStyles(styles)(ScimMappings));