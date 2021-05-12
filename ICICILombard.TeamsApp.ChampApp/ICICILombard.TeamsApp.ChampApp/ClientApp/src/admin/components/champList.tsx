import React from 'react';
import { SearchIcon, ComposeIcon } from '@fluentui/react-icons-northstar';

import { Flex, Button, Loader, FormInput, FormDatepicker } from "@fluentui/react-northstar";


import "./../styles.scss"

import { getAwardListAPI } from './../../apis/AwardListApi'

type MyState = {
    awardList?: any;
    formDate?: any;
    toDate?: any;
    findContact?: any;
    loading?: any
};


class ChampList extends React.Component<MyState> {
    state: MyState = {
        loading: true
    };



    componentDidMount() {
        this.getAwardListInitial()
    }

    getAwardListInitial() {
        const data = {
            "FromDate": "",
            "ToDate": "",
            "RecipentName": ""
        }
        this.getAwardList(data)
    }

    getAwardList(data: any) {

        getAwardListAPI(data).then((res) => {
            console.log("get award", res);
            if (res.status === 200) {
                this.setState({
                    awardList: res.data,
                    loading: false
                })
            }

        })
    }

    fromDate(e: any, v: any) {
        console.log(v.value);
        var date = new Date(v.value)
        let mnth = ("0" + (date.getMonth() + 1)).slice(-2)
        let day = ("0" + date.getDate()).slice(-2)
        let year = date.getFullYear()
        let finaldate = day + '/' + mnth + '/' + year
        this.setState({
            formDate: finaldate
        })
    }

    toDate(e: any, v: any) {
        var date = new Date(v.value)
        let mnth = ("0" + (date.getMonth() + 1)).slice(-2)
        let day = ("0" + date.getDate()).slice(-2)
        let year = date.getFullYear()
        let finaldate = day + '/' + mnth + '/' + year
        this.setState({
            toDate: finaldate
        })

    }

    findContact(e: any) {
        this.setState({
            findContact: e.target.value
        })

    }

    search() {
        const data = {
            "FromDate": this.state.formDate ? this.state.formDate : "",
            "ToDate": this.state.toDate ? this.state.toDate : "",
            "RecipentName": this.state.findContact ? this.state.findContact : ""
        }
        this.getAwardList(data)
    }

    render() {

        return (
            <div>

                {/* <div className="containterBox" > */}
                <Flex className="pt-1 " vAlign="end" gap="gap.medium">
                        

                    </Flex>
                    <div className="statisticsSerchDivButton">
                    <FormDatepicker label="From Date" onDateChange={(e, v) => this.fromDate(e, v)} />
                        <FormDatepicker label="To Date" onDateChange={(e, v) => this.toDate(e, v)} />
                        <FormInput label="Find Contact" name="Find Contact" placeholder="Find Contact" onChange={(e) => this.findContact(e)} />
                        <Button primary content="Search" icon={<SearchIcon />} styles={{ marginRight: "10px" }} onClick={() => this.search()} />
                        <Button content="Export" icon={<ComposeIcon />} />

                    </div>
                {/* </div> */}



                {/* </div> */}
                {!this.state.loading ? <div>
                    {this.state.awardList && !(this.state.awardList.length > 0) ? <table className="ViswasTable">

                        {this.state.awardList.map((e: any) => {
                            return <tr className="ViswasTableRow">
                                <td>
                                    <div className="tableTitle">
                                        Person Name
                                    </div>
                                    <div style={{ fontWeight: "bold" }}>
                                        {e.recipentEmail}
                                    </div>
                                </td>
                                <td style={{ textAlign: "end" }}>
                                    <img className="badgePageIcon" src={e.cardImage} />
                                </td>
                                <td style={{ textAlign: "start" }}>
                                    <div className="tableTitle">
                                        Award
                                    </div>
                                    <div style={{ fontWeight: "bold" }}>
                                        {e.cardName}
                                    </div>
                                </td>
                                <td style={{ paddingLeft: "25px" }}>
                                    <div className="tableTitle">
                                        Date
                                    </div>
                                    <div style={{ fontWeight: "bold" }}>
                                        {e.awardDate}

                                    </div>
                                </td>
                                <td style={{ paddingLeft: "25px" }}>
                                    <div className="tableTitle">
                                        Given By
                                    </div>
                                    <div style={{ fontWeight: "bold" }}>
                                        {e.awardedByName}
                                    </div>
                                </td>
                                <td style={{ textAlign: "end" }}>
                                    <div className="tableTitle">
                                        Viswas Behaviour
                                    </div>
                                    <div style={{ fontWeight: "bold" }}>
                                        {e.behaviourName}
                                    </div>
                                </td>
                            </tr>
                        })}


                    </table> : <div className="noDataText">No data Available</div>}</div> : <Loader label="Loading Data" styles={{ margin: "50px" }} />}



            </div>
        );
    }
}



export default ChampList;
