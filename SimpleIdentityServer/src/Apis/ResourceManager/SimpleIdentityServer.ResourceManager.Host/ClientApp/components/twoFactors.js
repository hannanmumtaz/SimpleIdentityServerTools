import React, { Component } from "react";
import { translate } from 'react-i18next';
import { Grid } from 'material-ui';
import { NavLink } from 'react-router-dom';

class TwoFactors extends Component {
    render() {
        var self = this;
        const { t } = self.props;
        return (<div className="block">
            <div className="block-header">
                <Grid container>
                    <Grid item md={5} sm={12}>
                        <h4>{t('twoFactorsTitle')}</h4>
                        <i>{t('twoFactorsShortDescription')}</i>
                    </Grid>
                    <Grid item md={7} sm={12}>                        
                        <ul className="breadcrumb float-md-right">
                            <li className="breadcrumb-item"><NavLink to="/">{t('websiteTitle')}</NavLink></li>
                            <li className="breadcrumb-item">{t('twoFactors')}</li>
                        </ul>
                    </Grid>
                </Grid>
            </div>
            <div className="card">
                <div className="body">
                    {t('aboutContent')}
                </div>
            </div>
        </div>);
    }
}

export default translate('common', { wait: process && !process.release })(TwoFactors);