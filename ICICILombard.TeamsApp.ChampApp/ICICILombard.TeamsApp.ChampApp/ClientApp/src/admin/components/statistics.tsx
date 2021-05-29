import React from 'react';

import { Header, Flex, Text, Button, Card, CardBody, FlexItem, FormDropdown, Loader } from "@fluentui/react-northstar";


import "./../styles.scss"
import { getAwardListByCardAPI, getAwardListByRecipentAPI } from './../../apis/AwardListApi'
import { getApplauseCardAPI } from "./../../apis/ApplauseCardApi"
import { getAwardedEmployeeAPI } from "./../../apis/AwardedEmployeeApi"

type MyState = {
    awardList?: any;
    findContact?: any;
    searchListInput?: any;
    searchListInputValue?:any;
    applauseCardList?: any;
    applauseCardId?: any;
    awardedEmployeeList?: any;
    awardedEmployeeName?: any;
    awardedEmployeeEmail?: any;
    searchByList?: any;
    selectedSearchByValue?: any;
    loading?: any;
};

class Statistics extends React.Component<MyState> {
    state: MyState = {
        searchByList: ["By Award", "By Employee"],
        loading: true
    };



    componentDidMount() {
        this.setState({
            selectedSearchByValue: this.state.searchByList[0]
        }, () => {
            this.getSearchListInput(this.state.selectedSearchByValue)
        })

    }

    selectSearchByValueFunction = (data: any) => {
        this.setState({
            selectedSearchByValue: data
        }, () => {
            this.getSearchListInput(this.state.selectedSearchByValue)
        })

    }


    //////////////////////////////// function for getting serarch list inputs /////////////////
    getSearchListInput(data: any) {
        if (data === 'By Award') {
            const data = {
                "CardId": 0
            }
            getApplauseCardAPI(data).then((res) => {
                // console.log("get ApplauseCard API", res.data);
                let result = res.data.map((a: any) => a.cardName)
                this.setState({
                    applauseCardList: res.data,
                    searchListInput: result,
                    searchListInputValue:result[0],
                    applauseCardId: res.data[0].cardId
                }, () => {
                    this.search()
                })

            })
        }
        else {
            getAwardedEmployeeAPI().then((res) => {
                // console.log("get Awarded Employee API", res);
                let AwardedEmployeeName = res.data.map((a: any) => a.employeeName)
                this.setState({
                    searchListInput: AwardedEmployeeName,
                    searchListInputValue:AwardedEmployeeName[0],
                    awardedEmployeeList: res.data,
                    awardedEmployeeEmail: res.data[0].employeeEmail,
                    awardedEmployeeName: res.data[0].employeeName

                }, () => {
                    this.search()
                })
            })
        }

    }


    /////////////////////// Api call for getting all the award ///////////////////////
    getAwardList(data: any) {
        if (this.state.selectedSearchByValue === 'By Award') {
            getAwardListByCardAPI(data).then((res: any) => {
                // console.log("get award", res.data);
                this.setState({
                    awardList: res.data,
                    loading: false
                })
            })
        }
        else {
            getAwardListByRecipentAPI(data).then((res: any) => {
                // console.log("get award", res.data);
                this.setState({
                    awardList: res.data,
                    loading: false
                })
            })

        }
    }

    awardInput = (data: any) => {
        this.setState({
            searchListInputValue:data
        })
        if (this.state.selectedSearchByValue === 'By Award') {
            this.state.applauseCardList.filter((e: any) => e.cardName === data).map((e: any) => {
                this.setState({
                    applauseCardId: e.cardId
                })
            })
        }
        else {
            this.state.awardedEmployeeList.filter((e: any) => e.employeeName === data).map((e: any) => {
                this.setState({
                    awardedEmployeeEmail: e.employeeEmail,
                    awardedEmployeeName: e.employeeName
                })
            })
        }
    }




////////////// Search Function ////////////////////////
    search() {
        this.setState({
            loading:true
        },()=>{
            if (this.state.selectedSearchByValue === 'By Award') {
                const data = {
                    "FromDate": "",
                    "ToDate": "",
                    "CardId": this.state.applauseCardId,
                }
                this.getAwardList(data)
            }
            else {
                const data = {
                    "FromDate": "",
                    "ToDate": "",
                    "RecipentEmail": this.state.awardedEmployeeName
                }
                this.getAwardList(data)
            }
        })
        
    }

    render() {

        return (
            <div style={{ margin: '0 auto' }}>

                {this.state.searchListInput && <Flex className="pt-1 pb-2" vAlign="end" gap="gap.medium">
                    <FormDropdown fluid
                        items={this.state.searchByList}
                        search={false}
                        placeholder={this.state.searchByList[0]}
                        className="statisticsDropDown"
                        onChange={(event, { value }) => this.selectSearchByValueFunction(value)}
                    />
                    <FormDropdown fluid
                        items={this.state.searchListInput}
                        // search={true}
                        value={this.state.searchListInputValue}
                        className="statisticsDropDown"
                        onChange={(event, { value }) => this.awardInput(value)}
                    />
                    <Button primary content="Search" onClick={() => this.search()} styles={{ width: "15%" }} />
                </Flex >}

                {!this.state.loading ? <div>
                    {this.state.awardList && (this.state.awardList.length > 0) ? <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gridGap: 20 }}>
                        {this.state.awardList.map((e: any) => {
                            return <Card fluid className="card pointer">
                                <CardBody>
                                    <img className="batch-statistics" src={e.cardImage} />
                                    <Flex vAlign="center" className="pt-1">
                                        {/* <Avatar
                                    image="https://fabricweb.azureedge.net/fabric-website/assets/images/avatar/RobertTolbert.jpg"
                                    label="Product Creative Director"
                                    name="Shirley Larkin"
                                /> */}
                                        <Flex column styles={{ padding: '5px' }}>
                                            <Text content={e.recipentName} weight="bold" styles={{ color: "black" }} />
                                            <Text content={e.recipentEmail} size="smallest" styles={{ color: "gray" }} />
                                            <Text content={e.cardName} size="medium" styles={{ color: "#F17E21", marginTop: "4px" }} />
                                        </Flex>
                                        <FlexItem push>
                                            <Header as="h1">{e.awardCount}</Header>
                                        </FlexItem>
                                    </Flex>
                                </CardBody>
                            </Card>
                        })}

                    </div> : <div className="noDataText"> No Data Available</div>}

                </div> : <Loader styles={{ margin: "50px" }} />}
            </div>
        );
    }
}



export default Statistics;
