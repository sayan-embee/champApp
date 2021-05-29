import React from 'react';
import { Text, Input, FormInput, FlexItem, Loader, Flex, Button } from "@fluentui/react-northstar";
import { PaperclipIcon } from '@fluentui/react-icons-northstar';

import * as microsoftTeams from "@microsoft/teams-js";

import "./../styles.scss"
import { addApplauseCardAPI, getApplauseCardAPI } from "./../../apis/ApplauseCardApi"

import Toggle from 'react-toggle'

interface MyState {
    cardId?: any;
    cardName?: any;
    cardImage?: any;
    IsActive?: any;
    editActiveStatusValue?: any;
    editNameValue?: any;
    editActiveStatus?: any;
    loading?: any;
    buttonDisabled?: any;
    base64Image?: any;
    cardData?: any
}

interface IViswasEditProps {
    history?: any;
    location?: any
}

class EditBadge extends React.Component<IViswasEditProps, MyState> {
    constructor(props: IViswasEditProps) {
        super(props);
        this.state = {
            loading: true,
            editActiveStatus: false,
            buttonDisabled: false
        };
    }



    componentDidMount() {
        const params = new URLSearchParams(this.props.location.search);
        this.setState({
            cardId: params.get('id'),
        }, () => {
            this.getApplauseCard(this.state.cardId)
        })
    }


    getApplauseCard(id: any) {
        const data = {
            "CardId": id
        }
        getApplauseCardAPI(data).then((res) => {
            this.setState({
                cardData: res.data,
                base64Image: null,
                loading: false
            })

        })
    }


    editActiveStatus = (e: any) => {
        this.setState({
            editActiveStatusValue: e.target.checked ? 1 : 0,
            editActiveStatus: true
        })
    }

    editName(e: any) {
        if (e.target.value.length === 0) {
            this.setState({
                buttonDisabled: true
            })
        }
        else {
            this.setState({
                buttonDisabled: false
            })
        }
        this.setState({
            editNameValue: e.target.value
        })
    }



    editFunction(data: any) {
        const Value = {
            "CardId": data.cardId,
            "CardName": this.state.editNameValue ? this.state.editNameValue : data.cardName,
            "CardImage": this.state.base64Image ? this.state.base64Image : data.cardImage,
            "IsActive": this.state.editActiveStatus ? this.state.editActiveStatusValue : data.isActive
        }
        this.addApplauseCard(Value)
    }

    addApplauseCard(data: any) {
        addApplauseCardAPI(data).then((res) => {
            if (res.data.successFlag === 1) {
                microsoftTeams.tasks.submitTask()
            }
        })
    }

    fileUpload() {
        (document.getElementById('upload') as HTMLInputElement).click()
    };

    onFileChoose(event: any) {
        this.getBase64(event.target.files[0], event.target.files[0].lastModified)
    }

    getBase64(file: any, name: any) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            // console.log('Photo', reader.result);
            this.setState({
                base64Image: reader.result,
            })
        };
        reader.onerror = function (error) {
            // console.log('Error: ', error);
        };
    }



    render() {

        return (
            <div style={{ margin: "25px", height: "200px" }}>
                {!this.state.loading ? <div style={{ paddingTop: '20px', marginTop: "10px" }}>
                    {this.state.cardData && <div>
                        {this.state.cardData.map((e: any) => {
                            return <div>
                                <FormInput
                                    label="Name"
                                    name="Name"
                                    id="Name"
                                    defaultValue={e.cardName}
                                    required fluid
                                    onChange={(e) => this.editName(e)}
                                    showSuccessIndicator={false}
                                />

                                <div style={{ display: "flex", flexDirection: "column", marginTop: "15px" }}>
                                    <Text> Change Icon</Text>
                                    <Input type="file" id="upload" style={{ display: 'none' }} onChange={value => this.onFileChoose(value)}></Input>


                                    <div style={{ marginTop: "10px" }}>
                                        <div onClick={() => this.fileUpload()} className='cameraBtn pointer'>
                                            <PaperclipIcon size="smallest" />
                                        </div>
                                        {this.state.base64Image ? <img src={this.state.base64Image} className="badgePageEditIcon" /> :
                                            <img src={e.cardImage} className="badgePageEditIcon" />}
                                    </div>

                                </div>

                                <div className="outerDivToggleRadioGroup editOuterDivToggleRadioGroup">
                                    <Text styles={{ marginRight: "10px" }}> Active </Text>
                                    <Toggle defaultChecked={e.isActive} icons={false} onChange={(e) => this.editActiveStatus(e)} ></Toggle>
                                </div>
                                <div className="taskmoduleButtonDiv">
                                    <Flex gap="gap.small">
                                        <FlexItem push>
                                            <Button disabled={this.state.buttonDisabled ? true : false} primary onClick={() => this.editFunction(e)}>
                                                Update
                                        </Button>
                                        </FlexItem>
                                    </Flex>
                                </div>


                            </div>
                        })}






                    </div>}


                </div> : <Loader styles={{ margin: "50px" }} />}

            </div>
        );
    }
}



export default EditBadge;
