import React, { Component } from "react";
import { ViewScopes } from './common';

class OpenidScopes extends Component {
	render() {
		return ( <ViewScopes type="openid" /> );
	}
}

export default OpenidScopes;