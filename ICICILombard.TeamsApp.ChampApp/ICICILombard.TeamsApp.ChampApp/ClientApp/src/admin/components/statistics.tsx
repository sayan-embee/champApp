import React from 'react';
import { SearchIcon } from '@fluentui/react-icons-northstar';

import { Header, Flex, Text, Button, Card, CardBody, FlexItem,  FormDropdown } from "@fluentui/react-northstar";


import "./../styles.scss"
import { getAwardListByCardAPI } from './../../apis/AwardListApi'
import { getApplauseCardAPI } from "./../../apis/ApplauseCardApi"
import { getAwardedEmployeeAPI } from "./../../apis/AwardedEmployeeApi"

type MyState = {
    awardList?: any;
    BadgeList?: any;
    findContact?: any;
    byAwardListInput?: any;
    CardId?: any;
    AwardedEmployeeList?:any;
    AwardedEmployeeName?:any;
    AwardedEmployeeEmail?:any
};

class Statistics extends React.Component<MyState> {
    state: MyState = {

    };



    componentDidMount() {
        this.getApplauseCard()
        this.getAwardedEmployee()
    }

    getAwardedEmployee(){
        getAwardedEmployeeAPI().then((res)=>{
            console.log("get Awarded Employee API",res);
            let AwardedEmployeeName = res.data.map((a: any) => a.employeeEmail)
            this.setState({
                AwardedEmployeeName:AwardedEmployeeName,
                AwardedEmployeeList:res.data
            })
        })
    }

    getApplauseCard() {
        const data = {
            "CardId": 0
        }
        getApplauseCardAPI(data).then((res) => {
            console.log("get ApplauseCard API", res.data);
            let result = res.data.map((a: any) => a.cardName)
            this.setState({
                BadgeList: res.data,
                byAwardListInput: result
            }, () => {
                console.log(this.state.BadgeList[0].cardId);
                this.getAwardListInitial()
            })

        })
    }

    getAwardListInitial() {
        const data = {
            "FromDate": "",
            "ToDate": "",
            "CardId": this.state.BadgeList[0].cardId
        }
        this.getAwardList(data)
    }


    getAwardList(data: any) {
        getAwardListByCardAPI(data).then((res: any) => {
            console.log("get award", res.data);
            this.setState({
                awardList: res.data
            })
        })
    }

    awardInput = (data: any) => {
        this.state.BadgeList.filter((e: any) => e.cardName === data).map((e: any) => {
            this.setState({
                CardId: e.cardId
            })
        })
    }

    employee = (data:any)=>{
        this.state.AwardedEmployeeList.filter((e: any) => e.employeeEmail === data).map((e: any) => {
            this.setState({
                AwardedEmployeeEmail: e.employeeName
            })
        })
    }

    selectSearch=(data:any)=>{
console.log(data);

    }

    search() {
        const data = {
            "FromDate": "",
            "ToDate": "",
            "CardId": this.state.CardId,
            //  "RecipentEmail":this.state.AwardedEmployeeEmail
        }
        this.getAwardList(data)
    }

    render() {

        return (
            <div style={{
                margin: '0 auto',
                padding: '20px',
                backgroundColor: '#ffffff',
            }}>

                {(this.state.byAwardListInput && this.state.AwardedEmployeeName) && <Flex className="pt-1 pb-2" vAlign="end" gap="gap.medium" styles={{justifyContent:"center"}}>
                     <FormDropdown fluid
                        items={["By award","By Employee"]}
                        search={false}
                        placeholder="Search By Employee"
                        styles={{width:"30%"}}
                        onChange={(event, { value }) => this.selectSearch(value)}
                    />
                     <FormDropdown fluid
                        items={this.state.byAwardListInput}
                        search={false}
                        placeholder={this.state.byAwardListInput[0]}
                        onChange={(event, { value }) => this.awardInput(value)}
                        styles={{width:"30%"}}
                    />
                    <Button primary content="Search" icon={<SearchIcon />} onClick={() => this.search()} styles={{width:"25%"}} />
                </Flex >}
                {this.state.awardList && (this.state.awardList.length>0) ? <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gridGap: 20 }}>
                     {this.state.awardList.map((e:any)=>{
                       return <Card className="card pointer active">
                        <CardBody>
                            <img className="batch-statistics" src={e.cardImage} />
                            <Flex vAlign="center" className="pt-1">
                                {/* <Avatar
                                    image="https://fabricweb.azureedge.net/fabric-website/assets/images/avatar/RobertTolbert.jpg"
                                    label="Product Creative Director"
                                    name="Shirley Larkin"
                                /> */}
                                <Flex styles={{ padding: '5px' }}>
                                    <Text content={e.recipentEmail} weight="bold" />
                                </Flex>
                                <FlexItem push>
                                    <Header as="h1">{e.awardCount}</Header>
                                </FlexItem>
                            </Flex>
                        </CardBody>
                    </Card>
                    }) }

                </div>:<div style={{justifyContent:"center"}}> No data available</div>}
                {/* <Text>Award by Employee</Text>
                <Flex gap="gap.large" className="pt-2 flex-wrap">
                    <Card className="card pointer">
                        <CardBody>
                            <Image className="batch-statistics" src="http://primeuniversity.net/gold.svg" />
                            <Flex vAlign="center" className="pt-1">
                                <Header as="h3" className="color-gold">Gold</Header>
                                <FlexItem push>
                                    <Header as="h1">8</Header>
                                </FlexItem>
                            </Flex>
                        </CardBody>
                    </Card>
                    <Card className="card pointer">
                        <CardBody>
                            <Image className="batch-statistics" src="http://primeuniversity.net/gold.svg" />
                            <Flex vAlign="center" className="pt-1">
                                <Header as="h3" className="color-gold">Gold</Header>
                                <FlexItem push>
                                    <Header as="h1">8</Header>
                                </FlexItem>
                            </Flex>
                        </CardBody>
                    </Card>
                </Flex> */}


            </div>
        );
    }
}



export default Statistics;
