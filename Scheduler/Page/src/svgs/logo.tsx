import { ReactElement } from "react";

interface ILogoProps {
    primaryColor: string; 
    secondaryColor: string;
    borderColor: string;
}

const defaultProps: ILogoProps = {
    primaryColor: "currentColor",
    secondaryColor: "currentColor",
    borderColor: "black",
}

const Logo = (props: ILogoProps): ReactElement => {
    return(
        <div style={{color: props.borderColor}}>
            <svg overflow="visible" viewBox="0 0 180 180" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                <path fill={props.primaryColor} d="M50.24 59.43h39.84c12.64 0 24.51 9.39 27.55 21.64 1.85 7.47 1.85 7.47-5.9 7.47h-9.26c-1.83 0-3-.47-3.63-2.5-1.41-4.55-4.72-6.83-9.49-6.84H23.58c-2 0-3.55.11-3.7 2.84-.79 14.27 1.4 27.86 8.5 40.45 9.4 16.66 23.29 27.67 41.53 33.3a65.17 65.17 0 0 0 20 2.6c15.85-.14 31.71 0 47.56 0h2.47c.82 0 2-.07 2.36 1s-.62 1.68-1.36 2.2a89.29 89.29 0 0 1-22.09 11.49 88.05 88.05 0 0 1-79.41-10.32c-17.78-12.2-29.82-28.61-35.84-49.39A86.5 86.5 0 0 1 .19 82.59a95.57 95.57 0 0 1 3.92-20.16c.7-2.32 2.38-3 4.75-3 13.79.07 27.58 0 41.38 0z"/>
                <path fill={props.primaryColor} d="M129.02 118.21H90.1c-15.16 0-27-10.16-29.58-25-.47-2.76.51-3.55 3-3.52 4 .06 8 .08 12 0a3.72 3.72 0 0 1 4 2.55c2 4.76 5.52 7 10.88 6.91 10.39-.1 20.79.6 31.17.31 10.59-.3 21.15 0 31.72.07 4.94 0 5.19-.2 5.57-5.07 1.63-20.78-4.82-38.77-19.19-53.77-14-14.57-31.21-21.58-51.42-21.43-15.76.11-31.51 0-47.27 0h-2.16c-.72 0-1.75 0-2-1s.51-1.53 1.19-2a105.65 105.65 0 0 1 14.73-8.41c20.53-8.94 41.65-10.32 63-3.78a86.28 86.28 0 0 1 33.44 19.36 87.84 87.84 0 0 1 28.62 73.24c-.48 6-1.9 11.77-3.22 17.59-.73 3.2-2.4 4-5.44 4-13.34-.11-26.73-.05-40.12-.05z"/>
                <path fill={props.secondaryColor} d="M119.18 148.43c-9.27 0-18.53-.08-27.8 0-14.06.15-26.87-3.78-37.94-12.36a58.35 58.35 0 0 1-21.37-31.64c-1-3.61-1.13-7.3-2-10.89-.76-3.09.32-4.32 3.39-4.39 4.32-.08 8.64 0 13 0 2 0 2.93.75 3.2 2.85 2.16 17.22 11.35 29 27.71 34.88a28 28 0 0 0 9.62 1.84c25.33-.07 50.66 0 76 0a26.51 26.51 0 0 1 3.09.08c1.94.23 2.54 1.4 1.6 3a95.42 95.42 0 0 1-10.8 15.36c-1.24 1.43-2.94 1.3-4.59 1.3h-33.11z"/>
                <path fill={props.secondaryColor} d="M58.97 29.84c10 0 20-.21 30 0 24.52.63 42.47 12.06 53.75 33.74 3.39 6.52 4.84 13.74 5.59 21.05.27 2.54-.66 3.9-3.43 3.84-3.91-.1-7.82-.06-11.73 0-2.51 0-3.66-1.06-3.93-3.64-1.54-14.31-9-24.79-21.47-31.54a36.73 36.73 0 0 0-17.79-4.37c-24.7.08-49.41 0-74.11 0h-2.78c-2 0-2.28-.94-1.45-2.57A76.23 76.23 0 0 1 21.79 31.3c1.21-1.42 2.81-1.52 4.5-1.52z"/>
            </svg>
        </div>
    );
}

Logo.defaultProps = defaultProps;
export default Logo;