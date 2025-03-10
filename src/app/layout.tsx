'use client';
// import type { Metadata } from "next";
import "./globals.css";
import { Layout } from "antd";
import { Content, Footer } from "antd/es/layout/layout";
// import Link from "next/link";
import type { Viewport } from "next";
import { useAuthStore } from "@/stores/authStore";
import Header from "@/components/Header";
import { useReaderAuthStore } from "@/stores/readerAuthStore";



export const viewport: Viewport = {
  themeColor: "#FFFFFF",
};


// export const metadata: Metadata = {
//   title: "Create Next App",
//   description: "Generated by create next app",
// };

// const items = [
//   {key: "home" , label : <Link href={"/"}>Home</Link>},
  // {key: "books" , label : <Link href={"/table"}>Table</Link>},
  // {key: "q1" , label : <Link href={"/q1"}>q1</Link>},
  // {key: "q2" , label : <Link href={"/q2"}>q2</Link>},
  // {key: "q3" , label : <Link href={"/q3"}>q3</Link>},
  // {key: "q4" , label : <Link href={"/q4"}>q4</Link>},
  // {key: "q56" , label : <Link href={"/q56"}>q56</Link>},
  // {key: "q7" , label : <Link href={"/q7"}>q7</Link>},
  // {key: "q8" , label : <Link href={"/q8"}>q8</Link>},
  // {key: "q9" , label : <Link href={"/q9"}>q9</Link>},
  // {key: "q10" , label : <Link href={"/q10"}>q10</Link>},
  // {key: "q11" , label : <Link href={"/q11"}>q11</Link>},
  // {key: "q12" , label : <Link href={"/q12"}>q12</Link>},
  // {key: "q13" , label : <Link href={"/q13"}>q13</Link>},
  // {key: "q14" , label : <Link href={"/q14"}>q14</Link>},
  // {key: "q15" , label : <Link href={"/q15"}>q15</Link>},
  // {key: "q16" , label : <Link href={"/q16"}>q16</Link>},
  // {key: "lab3" , label : <Link href={"/lab3"}>lab3</Link>},
//   {key: "login" , label : <Link href={"/login"}>login</Link>},
//   {key: "Reader login" , label : <Link href={"/readerLogin"}>R login</Link>},
//   {key: "addDirector" , label : <Link href={"/adddirector"}>addDirector</Link>},


//   {key: "about" , label : <Link href={"/about"}>About me</Link>},
// ]

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {

  const user = useAuthStore((state) => state.user);
  const reader = useReaderAuthStore((state) => state.reader);
  

  return (
    
    <html lang="en">
      {/* <Head>
          <link rel="manifest" href="/manifest.json"/>
        </Head> */}
      <body>
        <Layout style={{minHeight: "100vh"}}>
          {/* <Header>
            <Menu theme="dark" mode="horizontal" items = {items} style={{flex:1, minWidth: 0}}/>
          </Header> */}
          { user?.id && <Header></Header>}
          { reader?.id && <Header></Header>}
          <Content style={{padding: "0 48px"}}>
            {children}
          </Content>
          <Footer style={{textAlign: "center"}}>
            BookStore
          </Footer>
        </Layout>
        </body>
    </html>
  );
}
