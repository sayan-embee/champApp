import React from 'react';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';

import ChampList from '../components/champList'
import Statistics from '../components/statistics'

import "./../styles.scss"
type MyState = {
};
class AwardPage extends React.Component<MyState> {
    state: MyState = {
       
    };



    componentDidMount() {
    }



    render() {

        return (
            <div style={{
                margin: '0 auto',
                padding: '20px',
                backgroundColor: '#ffffff',
            }}>
                <Tabs>
                    <TabList>
                        <Tab>Champ List</Tab>
                        <Tab>Statistics</Tab>
                    </TabList>

                    <TabPanel>
                        <ChampList />
                    </TabPanel>
                    <TabPanel>
                        <Statistics />
                    </TabPanel>
                </Tabs>

            </div>
        );
    }
}



export default AwardPage;
