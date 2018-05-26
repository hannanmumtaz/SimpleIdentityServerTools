import React, { Component } from "react";
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { ScimService } from '../services';
import { withRouter } from 'react-router-dom';
import { NavLink } from 'react-router-dom';
import { translate } from 'react-i18next';
import { TextField , Button, Grid, IconButton, CircularProgress } from 'material-ui';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';

class ScimResources extends Component {
    constructor(props) {
        super(props);
        this.refresh = this.refresh.bind(this);
        this.state = {
            isLoading: false,
            page: 0,
            pageSize: 5,
            count: 0
        };
    }

    /**
    * Display the users & groups.
    */
    refresh() {
        var self = this;
        var startIndex = self.state.page * self.state.pageSize;
        self.setState({
            isLoading: true
        });
        var request = { startIndex: startIndex, count: self.state.pageSize};
        ScimService.searchUsers(request).then(function(result) {
            console.log(result);
        }).catch(function(e) {
            console.log(e);
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('scimResources')}</h4>
                        <i>{t('scimResourcesDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('scimResources')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <Grid container spacing={40}>
                <Grid item md={12}>
                    <div className="card">
                        { self.state.isLoading ? ( <CircularProgress /> ) : (
                            <div>
                                <div className="header">
                                    <h4 style={{display: "inline-block"}}>{t('scimUsers')}</h4>
                                </div>
                                <div className="body">

                                </div>
                            </div>
                        )}
                    </div>
                </Grid>
                <Grid item md={12}>
                    <div className="card">
                        { self.state.isLoading ? ( <CircularProgress /> ) : (
                            <div>
                                <div className="header">
                                    <h4 style={{display: "inline-block"}}>{t('scimGroups')}</h4>
                                </div>
                                <div className="body">

                                </div>
                            </div>
                        )}
                    </div>
                </Grid>
            </Grid>
        </div>);
    }

    componentDidMount() {
        var self = this;
        self.refresh();
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(ScimResources));