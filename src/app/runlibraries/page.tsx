"use client";
import React, { useEffect, useState } from "react";
import { Card, Button, Modal, Form, Input, Row, Col, message } from "antd";
import axios from "axios";
import { ExclamationCircleOutlined } from "@ant-design/icons";
import { API_BASE_URL } from "../../../const";

const { confirm } = Modal;

interface Library {
  id: string;
  name: string;
  address: string;
  description: string;
  readingRooms: { id: string; name: string }[];
  readers: { id: string; fullName: string }[];
}

const LibrariesPage: React.FC = () => {
  const [libraries, setLibraries] = useState<Library[]>([]);
  const [editingLibrary, setEditingLibrary] = useState<Library | null>(null);
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [form] = Form.useForm();

  // Fetch all libraries
  const fetchLibraries = async () => {
    setIsLoading(true);
    try {
      const response = await axios.get<Library[]>(`${API_BASE_URL}/library`);
      setLibraries(response.data);
    } catch (error) {
      message.error("Ошибка при загрузке библиотек");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchLibraries();
  }, []);

  // Open modal for editing library
  const openEditModal = (library: Library) => {
    setEditingLibrary(library);
    setIsEditModalVisible(true);
    form.setFieldsValue(library);
  };

  // Close the edit modal
  const closeEditModal = () => {
    setEditingLibrary(null);
    setIsEditModalVisible(false);
    form.resetFields();
  };

  // Update library
  const updateLibrary = async (values: Library) => {
    if (!editingLibrary) return;
    try {
      await axios.put(`${API_BASE_URL}/library/${editingLibrary.id}`, values);
      message.success("Библиотека успешно обновлена");
      closeEditModal();
      fetchLibraries();
    } catch (error) {
      message.error("Ошибка при обновлении библиотеки");
    }
  };

  // Confirm delete library
  const confirmDelete = (id: string) => {
    confirm({
      title: "Вы уверены, что хотите удалить эту библиотеку?",
      icon: <ExclamationCircleOutlined />,
      okText: "Да",
      cancelText: "Отмена",
      onOk: async () => {
        try {
          await axios.delete(`${API_BASE_URL}/library/${id}`);
          message.success("Библиотека успешно удалена");
          fetchLibraries();
        } catch (error) {
          message.error("Ошибка при удалении библиотеки");
        }
      },
    });
  };

  // Open modal for adding new library
  const openAddModal = () => {
    setIsAddModalVisible(true);
    form.resetFields();
  };

  // Close add modal
  const closeAddModal = () => {
    setIsAddModalVisible(false);
  };

  // Add new library
  const addLibrary = async (values: Library) => {
    try {
      const response = await axios.post(`${API_BASE_URL}/library`, values);
      message.success("Библиотека успешно добавлена");
      closeAddModal();
      fetchLibraries();
    } catch (error) {
      message.error("Ошибка при добавлении библиотеки");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      {/* Button to add a new library */}
      <Button type="primary" onClick={openAddModal} style={{ marginBottom: 20 }}>
        Добавить библиотеку
      </Button>

      <Row gutter={[16, 16]}>
        {libraries.map((library) => (
          <Col key={library.id} xs={24} sm={12} md={8} lg={6}>
            <Card
              title={library.name}
              bordered
              actions={[
                <Button
                  type="link"
                  onClick={() => openEditModal(library)}
                  key="edit"
                >
                  Изменить
                </Button>,
                <Button
                  type="link"
                  danger
                  onClick={() => confirmDelete(library.id)}
                  key="delete"
                >
                  Удалить
                </Button>,
              ]}
            >
              <p>{library.description}</p>
              <p><strong>Адрес:</strong> {library.address}</p>
              <p><strong>Читальные залы:</strong> {library.readingRooms.map(room => room.name).join(", ")}</p>
              <p><strong>Читатели:</strong> {library.readers.map(reader => reader.fullName).join(", ")}</p>
            </Card>
          </Col>
        ))}
      </Row>

      {/* Modal for editing library */}
      <Modal
        title="Изменение библиотеки"
        visible={isEditModalVisible}
        onCancel={closeEditModal}
        onOk={() => form.submit()}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={updateLibrary}>
          <Form.Item
            name="name"
            label="Название библиотеки"
            rules={[{ required: true, message: "Введите название библиотеки" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="address"
            label="Адрес"
            rules={[{ required: true, message: "Введите адрес" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="description"
            label="Описание"
            rules={[{ required: true, message: "Введите описание" }]}
          >
            <Input.TextArea />
          </Form.Item>
        </Form>
      </Modal>

      {/* Modal for adding new library */}
      <Modal
        title="Добавить новую библиотеку"
        visible={isAddModalVisible}
        onCancel={closeAddModal}
        onOk={() => form.submit()}
        okText="Добавить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={addLibrary}>
          <Form.Item
            name="name"
            label="Название библиотеки"
            rules={[{ required: true, message: "Введите название библиотеки" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="address"
            label="Адрес"
            rules={[{ required: true, message: "Введите адрес" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="description"
            label="Описание"
            rules={[{ required: true, message: "Введите описание" }]}
          >
            <Input.TextArea />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default LibrariesPage;
