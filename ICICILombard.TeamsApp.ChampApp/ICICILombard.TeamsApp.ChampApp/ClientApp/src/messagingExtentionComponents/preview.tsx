import React from 'react';
import {  Flex, Text, Card, CardBody,  Button } from "@fluentui/react-northstar";
import { ArrowLeftIcon } from '@fluentui/react-icons-northstar';
import "./../styles.scss"
import * as microsoftTeams from "@microsoft/teams-js";

import { sendAwardAPI } from '../apis/sendAwardApi'



interface IPreviewProps {
    history?: any;
    location?: any
}

interface IPreviewState {
    Details?: any;
    allData?: any;
    selectedBadgeDetails?:any;
    groupId?: any;
    token?: any;
    userId?: any;
    UPN?: any;
    awardedByName?: any
    channelName?:any
}

class Preview extends React.Component<IPreviewProps, IPreviewState> {

    constructor(props: IPreviewProps) {
        super(props);
        this.state = {
            allData: [],
            selectedBadgeDetails:[]
        };
    }




    componentDidMount() {
        if (this.props.location.state) {
            this.setState({
                allData: this.props.location.state.data[0],
                selectedBadgeDetails:[...this.state.selectedBadgeDetails,{"badgeName":this.props.location.state.data[0].badgeName,"badgeId":this.props.location.state.data[0].badgeId,"badgeImage":this.props.location.state.data[0].badgeImage}],
                groupId:this.props.location.state.groupId && this.props.location.state.groupId,
                token:this.props.location.state.token && this.props.location.state.token,
                userId:this.props.location.state.userId && this.props.location.state.userId,
                UPN:this.props.location.state.UPN && this.props.location.state.UPN,
                awardedByName: this.props.location.state.awardedByName && this.props.location.state.awardedByName,
                channelName: this.props.location.state.channelName && this.props.location.state.channelName,
              
            })
            console.log(this.props.location.state.data);
            
        }
    }

   send(){
       const data={
        "AwardedByEmail":this.state.UPN,
        "AwardedByName":this.state.awardedByName,
        "CardId":this.state.allData.badgeId,
        "CardName":this.state.allData.badgeName,
        "IsGroup":0,
        "IsTeam":1,
        "IsChat":0,
        "ChannelId":this.state.groupId,
        "ChannelName":this.state.channelName,
        "BehaviourId":this.state.allData.behavioursId?this.state.allData.behavioursId:0,
        "BehaviourName":this.state.allData.behaviours?this.state.allData.behaviours:"",
        "Notes":this.state.allData.reason,
        "Recipents":this.state.allData.Recipents
       }
       console.log(data);
       
       sendAwardAPI(data).then((res)=>{
           console.log("send award",res);
           if(res.data.successFlag===1){
                this.submitTask()
           }
       })
   }


    submitTask() {
       
        this.setState({
            Details:{"awardRecipients":this.state.allData.name,"badge":this.state.selectedBadgeDetails,"behaviour":this.state.allData.behaviours, "reason":this.state.allData.reason, "awardedByUserId":this.state.userId, "awardedByName":this.state.awardedByName, "awardedByEmail":this.state.UPN}
        },()=>{
            console.log("send button",this.state.Details);
            
            microsoftTeams.tasks.submitTask(this.state.Details)
        })
    }

    back() {
        this.props.history.push({pathname:'/details',state:{data:this.state.allData,token:this.state.token, groupId:this.state.groupId}})
    }

    render() {
        return (
            <div>
                <Card fluid className="containerCard">
                    <CardBody>
                        <Flex hAlign="center" vAlign="center" padding="padding.medium"><Text>What </Text> <Flex>
                                {this.state.allData.name && this.state.allData.name.map((e: any ) => {
                                    return <Text styles={{marginLeft:"5px"}} content={e.name} />
                                })}
                            </Flex><Text styles={{marginLeft:"5px"}}>will see</Text></Flex>
                        <Card fluid centered styles={{
                            display: 'block',
                            maxWidth: '350px',
                            margin: '0 auto',
                            borderRadius: '12px',
                            borderWidth: '1',
                            borderStyle: 'solid',
                            borderColor: '#F17E21',
                            textAlign: 'center',
                            backgroundColor: 'white',
                            padding: '15px',
                            ':hover': {
                                backgroundColor: 'white',
                                borderWidth: '1',
                                borderStyle: 'solid',
                                borderColor: '#F17E21',
                            },
                        }}>
                            <Flex styles={{
                                justifyContent: "center",
                                alignItems: "center",
                            }}>
                                {/* <Avatar
                                    image="https://fabricweb.azureedge.net/fabric-website/assets/images/avatar/RobertTolbert.jpg"
                                />
                                <Flex styles={{
                                    padding: '5px',
                                }}>
                                    <Text content="Himanshu Damania" weight="bold" />
                                </Flex> */}
                            </Flex>
                            <Flex hAlign="center" vAlign="center"><Text>Sent applause to</Text></Flex>
                            <Flex styles={{
                                justifyContent: "center",
                                alignItems: "center",
                                marginTop:'10px'
                                                            }}>
                                <Flex styles={{ marginBottom: "10px" }}>
                                {this.state.allData.name && this.state.allData.name.map((e: any ) => {
                                    return <Text content={e.name} weight="bold" styles={{ marginRight: "10px" }} />
                                })}
                            </Flex>
                            </Flex>
                            <Flex space="between" padding="padding.medium">
                                <Card selected centered styles={{
                                    width: '165px',
                                    margin: '0 auto',
                                    borderRadius: '6px',
                                    backgroundColor: this.state.allData.badgeColor,
                                    ':hover': {
                                        backgroundColor: `${this.state.allData.badgeColor} !important`,
                                    },
                                }}>
                                    {this.state.allData.badgeImage && <img src={this.state.allData.badgeImage} />}
                                    {this.state.allData.badgeName && <text className="badgeText">{this.state.allData.badgeName}</text>}
                                </Card>
                            </Flex>
                            {this.state.allData.behaviours && <div>
                                <Flex hAlign="center" vAlign="center" ><Text>Vishvas Behaviours</Text></Flex>
                                <Flex hAlign="center" vAlign="center" ><Text color="brand">{this.state.allData.behaviours}</Text></Flex>
                            </div>}
                            {this.state.allData.reason && <div>
                                <Flex hAlign="center" vAlign="center"  ><Text>Reason for applause</Text></Flex>
                                <Flex hAlign="center" vAlign="center"  ><Text color="brand">{this.state.allData.reason}</Text></Flex>
                            </div>}

                        </Card>



                    </CardBody>

                </Card>
                <div className="margin20">
                    <Flex space="between">
                        <Button onClick={() => this.back()} icon={<ArrowLeftIcon />} text content="Back" />
                        <Flex gap="gap.small">
                            <Button content="Cancel" onClick={() => microsoftTeams.tasks.submitTask()} />
                            <Button primary onClick={() => this.send()}>Send</Button>
                        </Flex>
                    </Flex>
                </div>


            </div>
        );
    }
}



export default Preview;

