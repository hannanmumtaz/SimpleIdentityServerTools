import React, { Component } from 'react'
import { View, Text } from 'react-native'
import { Toolbar, Drawer, Avatar, Divider, COLOR } from 'react-native-material-ui'
import { Container }  from '../common'


export default class DrawerMenu extends Component {
	constructor(props, context) {
		super(props, context);
	}
	
	render() {
		return (
			<Container>
				<Toolbar
					leftElement="chevron-left"
					onLeftElementPress={() => this.props.navigation.navigate('DrawerToggle') }
					centerElement='SimpleIdentityServer'
				/>
				<Drawer>
			        <Drawer.Header >
			            <Drawer.Header.Account
			                avatar={<Avatar text="A" />}
			                accounts={[
			                    { avatar: <Avatar text="B" /> },
			                    { avatar: <Avatar text="C" /> },
			                ]}
			                footer={{
			                    dense: true,
			                    centerElement: {
			                        primaryText: 'Reservio',
			                        secondaryText: 'business@email.com',
			                    },
			                    rightElement: 'arrow-drop-down',
			                }}
			            />
			        </Drawer.Header>
			        <Drawer.Section
			            divider
			            items={[
			                { icon: 'bookmark-border', value: 'Notifications' },
			                { icon: 'today', value: 'Calendar', active: true },
			                { icon: 'people', value: 'Clients' },
			            ]}
			        />
			        <Drawer.Section
			            title="Personal"
			            items={[
			                { icon: 'info', value: 'Info' },
			                { icon: 'settings', value: 'Settings' },
			            ]}
			        />
		      	</Drawer>
			</Container>
			
		);
	}
}