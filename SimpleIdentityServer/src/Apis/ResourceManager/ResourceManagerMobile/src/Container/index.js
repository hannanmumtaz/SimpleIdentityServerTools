import { View, StyleSheet } from 'react-native';
import React, { Component } from 'react';
import { PropTypes } from 'prop-types';
import { Toolbar, Button, Drawer, Avatar, COLOR } from 'react-native-material-ui';

const propTypes = {
    children: PropTypes.node.isRequired,
};
const styles = StyleSheet.create({
    container: {
        flex: 1,
        flexDirection: 'row'
    },
    drawer: {
        flex: 2,
        elevation: 4,
        backgroundColor: 'white',
    },
    content: {
        flex: 1
    }
});

class Container extends Component {
    render() {
        return (
            <View style={styles.container}>
                <View style={styles.drawer}>
                    <Drawer>
                        <Drawer.Header>
                            <Drawer.Header.Account
                                avatar={<Avatar text="A" />}
                                footer={{
                                    dense: true,
                                    centerElement: {
                                        primaryText: 'thabart',
                                        secondaryText: 'habarthierry@hotmail.fr',
                                    }
                                }}
                                />
                        </Drawer.Header>
                    </Drawer>
                </View>
                <View style={styles.content}>  
                    <Toolbar centerElement="SimpleIdentityServer" />                  
                    {this.props.children}
                </View>
            </View>
        );
    }
}

Container.propTypes = propTypes;

export default Container;