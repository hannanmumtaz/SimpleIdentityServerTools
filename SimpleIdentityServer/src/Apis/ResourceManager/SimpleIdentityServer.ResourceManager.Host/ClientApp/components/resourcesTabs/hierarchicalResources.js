import React, { Component } from "react";
import { withRouter } from 'react-router-dom';
import { translate } from 'react-i18next';
import $ from 'jquery';
import Constants from '../../constants';
window.jQuery = $;
jQuery = $;
require('jquery-ui-dist/jquery-ui.js');
require('../../../elfinder/js/elfinder.full.js');
import { SessionStore } from '../../stores';
import { SessionService } from '../../services';

class HierarchicalResources extends Component {
    constructor(props) {
        super(props);
        this.display = this.display.bind(this);
    }

    display() {        
        var authUrl = SessionStore.getSession().selectedAuth['url'];
        $(this.refs.elfinder).elfinder({
            url : Constants.hierarchicalResourcesBaseUrl + '/elfinder',
            authUrl: authUrl,
            profileUrl: Constants.profileBaseUrl,
            getIdToken: function() {
                return SessionService.getSession().token;
            },
            contextmenu: {
                cwd    : ['reload', 'back', '|', 'mkdir', 'mkfile', 'paste', '|', 'sort', '|', 'info'],
                navbar : ['open', '|', 'copy', 'cut', 'paste', 'duplicate', '|', 'rm', '|', 'permissions' ],
                files: ['getfile', '|', 'mkdir', '|', 'copy', 'cut', 'paste', 'duplicate', '|', 'rm', '|', 'rename', '|', 'permissions', 'protectresource' ]
            },
            uiOptions: {
                toolbar: [
                    ['back', 'forward'],
                    ['mkdir', 'mkfile'],
                    ['rm'],
                    ['rename'],
                    ['search'],
                    ['view', 'sort'],
                    ['protectresource', 'permissions']
                ]
            }
        });
    }

    render() {
        var self = this;
        const { t } = self.props;
        return (
            <div className="card">
                <div className="header">
                    <h4 style={{display: "inline-block"}}>{t('hierarchicalResources')}</h4>
                </div>
                <div className="body" style={{ overflow: "visible" }}>
                    <div ref="elfinder"></div>
                </div>
            </div>
        );
    }

    componentDidMount() {
        var self = this;
        SessionStore.addChangeListener(function() {
            self.display();
        });

        if (SessionStore.getSession().selectedOpenid) {
            self.display();
        }

    }
}

export default translate('common', { wait: process && !process.release })(withRouter(HierarchicalResources));