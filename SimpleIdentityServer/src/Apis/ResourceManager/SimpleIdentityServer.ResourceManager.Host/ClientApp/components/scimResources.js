import React, { Component } from "react";
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';
import { WebsiteService, SessionService } from '../services';
import { withRouter } from 'react-router-dom';
import { translate } from 'react-i18next';

import { TextField , Button } from 'material-ui';

class ScimResources extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        const { t } = this.props;
        return (<div className="block">
            <div className="block-header">
                <h4>{t('scimResources')}</h4>
            </div>
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-12">
                        <div className="card">
                            <div className="body">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>);
    }
}

export default translate('common', { wait: process && !process.release })(withRouter(ScimResources));