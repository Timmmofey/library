"use client";
import React, { useEffect, useState } from "react";
import { Card, Button, Modal, Form, Input, Row, Col, message, Select } from "antd";
import axios from "axios";
import { ExclamationCircleOutlined } from "@ant-design/icons";
import { API_BASE_URL } from "../../../const";

const { confirm } = Modal;

interface Library {
  id: string;
  name: string;
  address: string;
}

interface ReadingRoom {
  id: string;
  name: string;
  libraryId: string;
}

const ReadingRoomsPage: React.FC = () => {
  const [readingRooms, setReadingRooms] = useState<ReadingRoom[]>([]);
  const [libraries, setLibraries] = useState<Library[]>([]);
  const [editingRoom, setEditingRoom] = useState<ReadingRoom | null>(null);
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  // const [isLoading, setIsLoading] = useState(false);
  const [form] = Form.useForm();

  // Fetch libraries for the dropdown
  const fetchLibraries = async () => {
    try {
      const response = await axios.get<Library[]>(`${API_BASE_URL}/Library`);
      setLibraries(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке библиотек", error);
    }
  };

  // Fetch all reading rooms
  const fetchReadingRooms = async () => {
    // setIsLoading(true);
    try {
      const response = await axios.get<ReadingRoom[]>(`${API_BASE_URL}/api/ReadingRooms`);
      setReadingRooms(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке читальных залов", error);
    } finally {
      // setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchReadingRooms();
    fetchLibraries();
  }, []);

  // Open the edit modal
  const openEditModal = (room: ReadingRoom) => {
    setEditingRoom(room);
    setIsEditModalVisible(true);
    form.setFieldsValue(room);
  };

  // Close the edit modal
  const closeEditModal = () => {
    setEditingRoom(null);
    setIsEditModalVisible(false);
    form.resetFields();
  };

  // Update reading room
  const updateReadingRoom = async (values: ReadingRoom) => {
    if (!editingRoom) return;
    try {
      await axios.put(`${API_BASE_URL}/api/ReadingRooms/${editingRoom.id}`, values);
      message.success("Читальный зал успешно обновлен");
      closeEditModal();
      fetchReadingRooms();
    } catch (error) {
      console.error("Ошибка при обновлении читального зала", error);
    }
  };

  // Confirm delete reading room
  const confirmDelete = (id: string) => {
    confirm({
      title: "Вы уверены, что хотите удалить этот читальный зал?",
      icon: <ExclamationCircleOutlined />,
      okText: "Да",
      cancelText: "Отмена",
      onOk: async () => {
        try {
          await axios.delete(`${API_BASE_URL}/api/ReadingRooms/${id}`);
          message.success("Читальный зал успешно удален");
          fetchReadingRooms();
        } catch (error) {
          console.error("Ошибка при удалении читального зала", error);
        }
      },
    });
  };

  // Open the add modal
  const openAddModal = () => {
    setIsAddModalVisible(true);
    form.resetFields();
  };

  // Close the add modal
  const closeAddModal = () => {
    setIsAddModalVisible(false);
  };

  // Add new reading room
  const addReadingRoom = async (values: ReadingRoom) => {
    try {
      await axios.post(`${API_BASE_URL}/api/ReadingRooms`, values);
      message.success("Читальный зал успешно добавлен");
      closeAddModal();
      fetchReadingRooms();
    } catch (error) {
      console.error("Ошибка при добавлении читального зала", error);
    }
  };

  // Function to get the library name by its ID
  const getLibraryName = (libraryId: string) => {
    const library = libraries.find((lib) => lib.id === libraryId);
    return library ? library.name : "Не найдено";
  };

  return (
    <div style={{ padding: "20px" }}>
      {/* Button to add a new reading room */}
      <Button type="primary" onClick={openAddModal} style={{ marginBottom: 20 }}>
        Добавить читальный зал
      </Button>

      <Row gutter={[16, 16]}>
        {readingRooms.map((room) => (
          <Col key={room.id} xs={24} sm={12} md={8} lg={6}>
            <Card
              title={room.name}
              bordered
              actions={[
                <Button type="link" onClick={() => openEditModal(room)} key="edit">
                  Изменить
                </Button>,
                <Button type="link" danger onClick={() => confirmDelete(room.id)} key="delete">
                  Удалить
                </Button>,
              ]}
            >
              {/* Show the library name instead of libraryId */}
              <p>Библиотека: {getLibraryName(room.libraryId)}</p>
            </Card>
          </Col>
        ))}
      </Row>

      {/* Edit Modal */}
      <Modal
        title="Изменение читального зала"
        visible={isEditModalVisible}
        onCancel={closeEditModal}
        onOk={() => form.submit()}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={updateReadingRoom}>
          <Form.Item name="name" label="Название" rules={[{ required: true, message: "Введите название" }]}>
            <Input />
          </Form.Item>
          <Form.Item
            name="libraryId"
            label="Библиотека"
            rules={[{ required: true, message: "Выберите библиотеку" }]}
          >
            <Select>
              {libraries.map((library) => (
                <Select.Option key={library.id} value={library.id}>
                  {library.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>

      {/* Add Modal */}
      <Modal
        title="Добавить новый читальный зал"
        visible={isAddModalVisible}
        onCancel={closeAddModal}
        onOk={() => form.submit()}
        okText="Добавить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={addReadingRoom}>
          <Form.Item name="name" label="Название" rules={[{ required: true, message: "Введите название" }]}>
            <Input />
          </Form.Item>
          <Form.Item
            name="libraryId"
            label="Библиотека"
            rules={[{ required: true, message: "Выберите библиотеку" }]}
          >
            <Select>
              {libraries.map((library) => (
                <Select.Option key={library.id} value={library.id}>
                  {library.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default ReadingRoomsPage;
