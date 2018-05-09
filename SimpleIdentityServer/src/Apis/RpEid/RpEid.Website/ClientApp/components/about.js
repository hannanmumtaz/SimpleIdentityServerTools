import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid, Button, Typography, CircularProgress } from 'material-ui';

class About extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        var self = this;
        const { t, classes } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('aboutTitle')}</h4>
                        <i>{t('aboutDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item">{t('websiteTitle')}</li>
                            <li className="breadcrumb-item">{t('about')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="header">
                    <h4 style={{ display: "inline-block" }}>{t('about')}</h4>
                </div>
                <div className="body">
                </div>
            </div>
        </div>);
    }
}

export default translate('common', { wait: process && !process.release })(About);