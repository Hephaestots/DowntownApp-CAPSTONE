import { LoadingButton } from "@mui/lab";
import { Stack, Typography, Button, Divider, InputAdornment } from "@mui/material";
import { Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import FormContainer from "../../app/common/form/FormContainer";
import TextInput from "../../app/common/form/TextInput";
import { useStore } from "../../app/stores/store";
import * as Yup from 'yup';
import { UserFormValues } from "../../app/models/user";
import FormValidationError from "../../app/common/form/FormValidationError";
import AlternateEmailIcon from '@mui/icons-material/AlternateEmail';
import LockIcon from '@mui/icons-material/Lock';

function LoginForm() {

    /** Validation schema using Yup. */
    const validationSchema = Yup.object<UserFormValues>({
        email: Yup.string().required().email(),
        password: Yup.string().required()
    });

    const { userStore } = useStore();

    return (
        <FormContainer
            title='Login'
            form={
                <Formik
                    initialValues={{email: '', password: '', error: null}}
                    onSubmit={(values, { setErrors }) => 
                        userStore.login(values)
                            .catch(error => setErrors({error: 'Invalid email or password'}))
                    }
                    validationSchema={validationSchema}
                >
                    {({handleSubmit, isSubmitting, errors}) => (
                        <Form 
                            className='ui-form'
                            onSubmit={handleSubmit}
                            autoComplete='off'
                        >
                            <Stack direction='column' spacing={3}>   
                                <TextInput 
                                    placeholder='Email' 
                                    name='email'
                                    InputProps={{
                                        endAdornment: (
                                            <InputAdornment position='start'>
                                                <AlternateEmailIcon fontSize="small" />
                                            </InputAdornment>
                                        )
                                    }}
                                />
                                <TextInput 
                                    placeholder='Password' 
                                    name='password' 
                                    type='password'
                                    InputProps={{
                                        endAdornment: (
                                            <InputAdornment position='start'>
                                                <LockIcon fontSize="small" />
                                            </InputAdornment>
                                        )
                                    }} 
                                />
                                <Divider sx={{width:'50%'}} />
                                {errors.error && <FormValidationError error={errors.error ?? ''} />}
                                <Stack direction='row' spacing={2}>
                                    <LoadingButton 
                                        loading={isSubmitting}
                                        variant='contained' 
                                        fullWidth 
                                        type="submit"
                                    >
                                        <Typography fontFamily='monospace'>Login</Typography>
                                    </LoadingButton>
                                    <Button component={Link} to='/' color="warning" variant="contained" fullWidth>
                                        <Typography fontFamily='monospace'>Cancel</Typography>
                                    </Button>
                                </Stack>
                            </Stack>
                        </Form>
                    )}
                </Formik>
            }
            minWidth={375}
        />
    );
};

/** Every time we need to use any `store` object, we need the observable transformation. */
export default observer(LoginForm);