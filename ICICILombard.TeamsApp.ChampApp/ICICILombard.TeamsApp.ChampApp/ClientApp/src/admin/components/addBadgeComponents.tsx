import React from 'react';
import { Input, Flex, Button, Card, CardBody, FormInput, Form, FlexItem, Text } from "@fluentui/react-northstar";

import { PaperclipIcon } from '@fluentui/react-icons-northstar';

import * as microsoftTeams from "@microsoft/teams-js";

import "./../styles.scss"
import { addApplauseCardAPI } from "./../../apis/ApplauseCardApi"


interface MyState {
    addNewInputName?: any;
    base64Image?: any;
    loading?: any
}

interface IBadgeAddProps {
}

class AddBadge extends React.Component<IBadgeAddProps, MyState> {
    constructor(props: IBadgeAddProps) {
        super(props);
        this.state = {
        };
    }



    componentDidMount() {
    }

    fileUpload() {
        (document.getElementById('upload') as HTMLInputElement).click()
    };

    onFileChoose(event: any) {
        console.log("check", event.target.files[0], event.target.files[0].lastModified);
        this.getBase64(event.target.files[0], event.target.files[0].lastModified)
    }

    getBase64(file: any, name: any) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            console.log('Photo', reader.result);
            this.setState({
                base64Image: reader.result,
            })
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
    }

    addNewInput(event: any) {
        this.setState({
            addNewInputName: event.target.value
        })
    }

    addNew() {
        const data = {
            "CardId": 0,
            "CardName": this.state.addNewInputName,
            "CardImage": this.state.base64Image,
            "IsActive": 1

        }
        this.addApplauseCard(data)
        console.log("addNew badge", data);
    }


    addApplauseCard(data: any) {
        addApplauseCardAPI(data).then((res) => {
            if (res.data.successFlag === 1) {
                microsoftTeams.tasks.submitTask()
            }

        })
    }

    render() {

        return (
            <div style={{ margin: "25px" }}>
                <Card fluid styles={{
                    display: 'block',
                    backgroundColor: 'transparent',
                    padding: '0',
                    marginBottom: '40px',
                    marginTop: "10px",
                    ':hover': {
                        backgroundColor: 'transparent',
                    },
                }}>
                    <CardBody>

                        <Form styles={{
                            paddingTop: '25px',
                            marginBottom: "10px"
                        }}>
                            <FormInput
                                label="Name"
                                name="Name"
                                id="Name"
                                required fluid
                                onChange={(e) => this.addNewInput(e)}
                                showSuccessIndicator={false}
                            />


                        </Form>
                    </CardBody>
                    <div style={{ display: "flex", flexDirection: "column" }}>
                        <Text> Attach Icon</Text>
                        <Input type="file" id="upload" style={{ display: 'none' }} onChange={value => this.onFileChoose(value)}></Input>
                        <div>
                            <div onClick={() => this.fileUpload()} className='cameraBtn pointer'>
                                <PaperclipIcon size="smallest" />
                            </div>
                            {this.state.base64Image && <img src={this.state.base64Image} className="badgePageEditIcon" />}

                        </div>
                    </div>

                </Card>
                <div className="taskmoduleButtonDiv">
                    <Flex gap="gap.small">
                        <FlexItem push>
                            <Button disabled={(this.state.addNewInputName && this.state.base64Image) ? false : true} primary onClick={() => this.addNew()}>
                                Add New
                  </Button>
                        </FlexItem>
                    </Flex>
                </div>

            </div>
        );
    }
}



export default AddBadge;
