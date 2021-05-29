import React from 'react';

import { Button, Loader, FormDatepicker, Text, FormInput, Flex } from "@fluentui/react-northstar";
import { CSVLink } from "react-csv";

import "./../styles.scss"

import { getAwardListAPI } from './../../apis/AwardListApi'

type MyState = {
    awardList?: any;
    formDate?: any;
    toDate?: any;
    findContact?: any;
    loading?: any;
    downloadData?:any;
};


class ChampList extends React.Component<MyState> {
    state: MyState = {
        loading: true

    }

    componentDidMount() {
        this.search()
    }

    search() {
        this.setState({
            loading:true
        })
        const data = {
            "FromDate": this.state.formDate ? this.state.formDate : "",
            "ToDate": this.state.toDate ? this.state.toDate : "",
            "RecipentName": this.state.findContact ? this.state.findContact : ""
        }
        this.getAwardList(data)
    }


    getAwardList(data: any) {
        getAwardListAPI(data).then((res) => {
            // console.log("get award", res);
            if (res.status === 200) {
                const downloadDataList = res.data.map((e: any) => {
                    let b = {
                        "Awarded By": e.awardedByName + ' ( ' + e.awardedByEmail + ' )',
                        "Applaud Card":  e.cardName,
                        "Awarded On": e.awardDate,
                        "Values/Behaviors": e.behaviourName,
                        "Received By": e.recipentName + ' ( ' + e.recipentEmail + ' )',
                        "Notes": e.notes
                    }
                    return b
                })
                this.setState({
                    awardList: res.data,
                    loading: false,
                    downloadData:downloadDataList
                })
            }

        })
    }

    fromDate(e: any, v: any) {
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

    

    render() {

        return (
            <div>

    
                <Flex className="pt-1" vAlign="end" gap="gap.smaller">
                    <FormDatepicker label="From Date" onDateChange={(e, v) => this.fromDate(e, v)} styles={{boxShadow: 'rgba(157, 150, 150, 0.15) 0px 2px 0px 0px'}} />
                    <FormDatepicker label="To Date" onDateChange={(e, v) => this.toDate(e, v)}  styles={{boxShadow: 'rgba(157, 150, 150, 0.15) 0px 2px 0px 0px'}}/>
                    <FormInput label="Find Contact" name="Find Contact" placeholder="Find Contact" onChange={(e) => this.findContact(e)} className="champListFindContact" />
                    <Button primary content="Search" onClick={() => this.search()}  />
                    <Button disabled={(this.state.downloadData && this.state.downloadData.length > 0) ? false : true} content="Export">
                        {this.state.downloadData && <CSVLink data={this.state.downloadData} filename={"reports-file" + new Date().toDateString() + ".csv"}>Export</CSVLink>}
                    </Button>    
                </Flex>

                {!this.state.loading ? <div>
                    {this.state.awardList && (this.state.awardList.length > 0) ? <table className="ViswasTable">
                        <tr>
                            <th className="tableTitle">Received By</th>
                            <th>Award</th>
                            <th style={{ paddingRight: '25px' }}> Date</th>
                            <th style={{ paddingLeft: '54px' }}> Awarded By</th>
                            <th style={{ paddingRight: '15px' }}>Values/Behaviors</th>
                        </tr>
                        {this.state.awardList.map((e: any) => {
                            return <tr className="ViswasTableRow">
                                <td>
                                    {/* <div style={{ fontWeight: "bold" }}> */}
                                    {e.recipentName}
                                    {/* </div> */}
                                </td>
                                <td >

                                    <div className="reportListImageDiv">
                                        <img className="badgePageIcon" src={e.cardImage} />
                                        <Text>{e.cardName}</Text>
                                    </div>
                                </td>
                                <td >
                                    {e.awardDate}
                                </td>
                                <td style={{ paddingLeft: '54px' }}>
                                    {e.awardedByName}
                                </td>
                                <td >
                                    {e.behaviourName}
                                </td>
                            </tr>
                        })}


                    </table> : <div className="noDataText">No Data Available</div>}</div> : <Loader styles={{ margin: "50px" }} />}



            </div>
        );
    }
}



export default ChampList;
