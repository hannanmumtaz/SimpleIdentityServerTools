import React, { Component } from "react";
import { withRouter } from 'react-router-dom';
import { translate } from 'react-i18next';
import $ from 'jquery';

class UserProfileTab extends Component {
    constructor(props) {
        super(props);
    }

    render() {
    	return (<div></div>);
    }
}


export default translate('common', { wait: process && !process.release })(withRouter(UserProfileTab));