import { List, Paper, Typography } from '@mui/material';
import ListItem from '@mui/material/ListItem/ListItem';
import { useTheme } from '@mui/material/styles';
import { Review } from '../../../app/models/event';

// TEMPORARY.
// const faces = [
//     "http://i.pravatar.cc/300?img=1",
//     "http://i.pravatar.cc/300?img=2",
//     "http://i.pravatar.cc/300?img=3",
//     "http://i.pravatar.cc/300?img=4"
// ];

interface Props {
    ratings: Review[];
}

function EventRatingReviews({ ratings }: Props) {
    const theme = useTheme();

    console.log(ratings);
    
    return (
        <>   
            <Typography
                sx={{ 
                    display: 'inline',
                    textDecoration: 'none',
                    fontFamily:'monospace'
                }}
                component="span"
                variant="h6"
                color="text.secondary"
            >
                Reviews
            </Typography>
            <Paper 
                sx={{
                    textAlign: 'left',
                    padding: theme.spacing(2)
                }} 
                elevation={3}
            >
                <List>
                    {ratings && ratings.map((rating, i) => {
                        return (
                            <ListItem key={i}>
                                <Typography>{rating.review}</Typography>    
                            </ListItem>
                        );
                    })}
                </List>
            </Paper>
        </>
    );
};

export default EventRatingReviews;