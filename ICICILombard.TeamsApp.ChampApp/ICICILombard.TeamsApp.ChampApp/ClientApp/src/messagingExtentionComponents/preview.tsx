import React from 'react';
import { Flex, Text, Card, CardBody, Button, Avatar, Loader } from "@fluentui/react-northstar";

import "./../styles.scss"
import * as microsoftTeams from "@microsoft/teams-js";

import { sendAwardAPI } from '../apis/sendAwardApi'

const baseUrl = window.location.origin
const backImage = baseUrl + "/images/left-arrow.svg"


interface IPreviewProps {
    history?: any;
    location?: any
}

interface IPreviewState {
    Details?: any;
    allData?: any;
    selectedBadgeDetails?: any;
    token?: any;
    userId?: any;
    UPN?: any;
    awardedByName?: any;
    awardedByImage?:any;
    channelName?: any;
    userObjectId?: any;
    groupId?: any;
    teamId?: any;
    teamName?:any;
    channelId?: any;
    chatId?: any;
    isGroup?: any;
    isTeam?: any;
    isChat?: any;
    loading?:any;
}

class Preview extends React.Component<IPreviewProps, IPreviewState> {

    constructor(props: IPreviewProps) {
        super(props);
        this.state = {
            allData: [],
            selectedBadgeDetails: []
        };
    }




    componentDidMount() {
        if (this.props.location.state) {
            this.setState({
                allData: this.props.location.state.data[0],
                selectedBadgeDetails: [...this.state.selectedBadgeDetails, { "badgeName": this.props.location.state.data[0].badgeName, "badgeId": this.props.location.state.data[0].badgeId, "badgeImage": this.props.location.state.data[0].badgeImage }],
                groupId: this.props.location.state.groupId && this.props.location.state.groupId,
                chatId: this.props.location.state.chatId && this.props.location.state.chatId,
                teamId: this.props.location.state.teamId && this.props.location.state.teamId,
                teamName: this.props.location.state.teamName && this.props.location.state.teamName,
                channelId: this.props.location.state.channelId && this.props.location.state.channelId,
                userObjectId: this.props.location.state.userObjectId && this.props.location.state.userObjectId,
                token: this.props.location.state.token && this.props.location.state.token,
                userId: this.props.location.state.userId && this.props.location.state.userId,
                UPN: this.props.location.state.UPN && this.props.location.state.UPN,
                awardedByName: this.props.location.state.awardedByName && this.props.location.state.awardedByName,
                awardedByImage: this.props.location.state.awardedByImage && this.props.location.state.awardedByImage,
                channelName: this.props.location.state.channelName && this.props.location.state.channelName,
                isGroup: this.props.location.state.isGroup && this.props.location.state.isGroup,
                isTeam: this.props.location.state.isTeam && this.props.location.state.isTeam,
                isChat: this.props.location.state.isChat && this.props.location.state.isChat,

            })
        }
    }

    send() {
        this.setState({
            loading:true
        })
        const data = {
            "AwardedByEmail": this.state.UPN,
            "AwardedByName": this.state.awardedByName,
            "CardId": this.state.allData.badgeId,
            "CardName": this.state.allData.badgeName,
            "IsGroup": this.state.isGroup,
            "IsTeam": this.state.isTeam,
            "IsChat": this.state.isChat,
            "GroupId": this.state.groupId ? this.state.groupId : "",
            "ChatId": this.state.chatId ? this.state.chatId : "",
            "TeamId":this.state.teamId ? this.state.teamId : "",
            "ChannelId": this.state.channelId ? this.state.channelId : "",
            "UserObjectId": this.state.userObjectId,
            "ChannelName": this.state.channelName?this.state.channelName:"",
            "BehaviourId": this.state.allData.behavioursId ? this.state.allData.behavioursId : 0,
            "BehaviourName": this.state.allData.behaviours ? this.state.allData.behaviours : "",
            "Notes": this.state.allData.reason,
            "Recipents": this.state.allData.Recipents,
            "TeamName": this.state.teamName ? this.state.teamName : ""
        }
        // console.log(data);
        this.submitTask(data);
        /*sendAwardAPI(data).then((res) => {
            console.log("send award response", res);
            if (res.data.successFlag === 1) {
                this.submitTask()
            }
        })*/
    }


    submitTask(data:any) {

        this.setState({
            Details: { award:data,"awardRecipients": this.state.allData.name, "badge": this.state.selectedBadgeDetails, "behaviour": this.state.allData.behaviours ? this.state.allData.behaviours : "", "reason": this.state.allData.reason, "awardedByUserId": this.state.userId, "awardedByName": this.state.awardedByName, "awardedByEmail": this.state.UPN }
        }, () => {
            console.log("send button", this.state.Details);
            microsoftTeams.tasks.submitTask(this.state.Details)
        })
    }

    back() {
        this.props.history.push({
            pathname: '/details', state: {
                data: this.state.allData,
                token: this.state.token,
                groupId: this.state.groupId,
                chatId: this.state.chatId,
                teamId: this.state.teamId,
                channelId: this.state.channelId,
                userObjectId: this.state.userObjectId,
                userId: this.state.userId,
                UPN: this.state.UPN,
                teamName:this.state.teamName
            }
        })
    }

    render() {
        return (
            <div>
                <Card fluid className="containerCard">
                    <CardBody>
                        <Flex hAlign="center" vAlign="center" padding="padding.medium"><Text>This is what </Text> <Flex>
                            {this.state.allData.name && this.state.allData.name.map((e: any) => {
                                return <Text styles={{ marginLeft: "5px" }} content={e.name} />
                            })}
                        </Flex><Text styles={{ marginLeft: "5px" }}>will see</Text></Flex>
                        <Card fluid centered styles={{
                            display: 'block',
                            maxWidth: '350px',
                            margin: '0 auto',
                            borderRadius: '12px',
                            borderWidth: '1',
                            borderStyle: 'solid',
                            borderColor: '#F17E21',
                            textAlign: 'center',
                            backgroundColor: 'transparent',
                            padding: '15px',
                            ':hover': {
                                backgroundColor: 'transparent',
                                borderWidth: '1',
                                borderStyle: 'solid',
                                borderColor: '#F17E21',
                            },
                        }}>
                            <Flex styles={{ justifyContent: "center", alignItems: "center", marginBottom: "7px"}}>
                                {(this.state.awardedByImage !== "")?<Avatar styles={{marginRight: '8px' }} image={this.state.awardedByImage}/>:<Avatar styles={{marginRight: '8px' }} image={baseUrl+"/images/userImage.png"}/>}
                                {this.state.awardedByName && <Flex styles={{padding: '5px' }}><Text weight="bold"> {this.state.awardedByName}</Text></Flex>}
                            </Flex>
                            <Flex hAlign="center" vAlign="center"><Text styles={{color:"grey", marginLeft:"30px"}}>Sent applause to</Text></Flex>
                            <Flex styles={{justifyContent: "center",alignItems: "center",marginTop: '7px'}}>
                                <Flex column styles={{ marginBottom: "7px" }}>
                                    {this.state.allData.name && this.state.allData.name.map((e: any) => {
                                        return <Flex styles={{ justifyContent:"flex-start", alignItems: "center", marginTop:"5px"}}>
                                        {(e.photo !== "")?<Avatar styles={{marginRight: '8px' }} image={e.photo}/>:<Avatar styles={{marginRight: '8px' }} image={baseUrl+"/images/userImage.png"}/>}
                                        <Flex styles={{padding: '5px' }}><Text weight="bold"> {e.name}</Text></Flex>
                                    </Flex>
                                    })}
                                </Flex>
                            </Flex>
                            <Flex space="between" padding="padding.medium">
                                <Card selected centered styles={{
                                    width: '165px',
                                    margin: '0 auto',
                                    borderRadius: '6px',
                                    backgroundColor: 'antiquewhite',
                                    ':hover': {
                                        backgroundColor: 'antiquewhite',
                                    },
                                }}>
                                    {this.state.allData.badgeImage && <img src={this.state.allData.badgeImage} />}
                                    {this.state.allData.badgeName && <text className="badgeText">{this.state.allData.badgeName}</text>}
                                </Card>
                            </Flex>
                            {this.state.allData.behaviours && <div>
                                <Flex hAlign="center" vAlign="center" ><Text>Values/Behaviors</Text></Flex>
                                <Flex hAlign="center" vAlign="center" ><Text color="brand">{this.state.allData.behaviours}</Text></Flex>
                            </div>}
                            {this.state.allData.reason && <div style={{ marginTop: "10px" }}>
                                <Flex hAlign="center" vAlign="center"  ><Text>Reason for applause</Text></Flex>
                                <Flex hAlign="center" vAlign="center"  ><Text color="brand">{this.state.allData.reason}</Text></Flex>
                            </div>}

                        </Card>



                    </CardBody>

                </Card>
                <div className="margin20">
                    <Flex space="between">
                        <div className="backButton pointer backButtonMessagingExtention" onClick={() => this.back()}>
                            <img src={backImage} /> <Text size="medium" >Back</Text>
                        </div>
                        <Flex gap="gap.small">
                            <Button content="Cancel" onClick={() => microsoftTeams.tasks.submitTask()} />
                            <Button primary onClick={() => this.send()}>{!this.state.loading?<Text styles={{ padding: "10px" }}>Send</Text>:<Loader size="smallest"  styles={{ margin: "10px", color:"white" }} />}</Button>
                        </Flex>
                    </Flex>
                </div>


            </div>
        );
    }
}



export default Preview;

