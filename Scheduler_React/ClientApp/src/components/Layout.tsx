import React, { ReactNode } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

export const metadata = {
  title: 'Next.js',
  description: 'Generated by Next.js',
}

export default function Layout({children}: { children: ReactNode }) {
  return (
    <div>
      <NavMenu />
      <Container tag="main">
        {children}
      </Container>
    </div>
  );
}

