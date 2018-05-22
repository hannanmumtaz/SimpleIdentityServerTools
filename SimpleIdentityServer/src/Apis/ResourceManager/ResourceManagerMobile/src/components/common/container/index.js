import React, { Component, PropTypes } from 'react';
import { View, StyleSheet, StatusBar } from 'react-native';

class Container extends Component {
	render() {
		return (
			<View style={{ flex: 1 }}>
				<StatusBar backgroundColor="rgba(0, 0, 0, 0.2)" translucent={true} barStyle="dark-content" />
				{this.props.children}
			</View>
		);
	}
}

export default Container;